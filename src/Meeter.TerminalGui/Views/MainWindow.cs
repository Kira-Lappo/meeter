using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Meeter.TerminalGui.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public class MainWindow : WindowFor<MainWindowViewModel>
{
    private readonly CompositeDisposable _disposable = new();

    public MainWindow(MainWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        Initialize();

        ViewModel.GenerateDummyData.Execute()
            .Wait();
    }

    public TableView MeetingsList { get; set; }

    private void Initialize()
    {
        Width              = Dim.Fill();
        Height             = Dim.Fill();
        X                  = 0;
        Y                  = 0;
        Modal              = false;
        Border.BorderStyle = BorderStyle.Single;
        TextAlignment      = TextAlignment.Left;

        this.OneWayBind(ViewModel,
                vm => vm.SelectedPeriodDateTime,
                v => v.Title,
                dateTime => $"Meetings for {dateTime:yyyy-MMMM-dd, dddd}")
            .DisposeWith(_disposable);

        MeetingsList = new TableView
        {
            X             = 0,
            Y             = 0,
            Width         = Dim.Fill(),
            Height        = Dim.Fill(),
            MultiSelect   = false,
            FullRowSelect = true,
            ColumnOffset = 2,
        };

        MeetingsList.Table = CreateMeetingsDataTable();

        MeetingsList.CellActivated += args =>
        {
            var row = args.Table.Rows[args.Row];
            var id = row.Field<Guid>(nameof(MeetingViewModel.Id));
            var item = ViewModel.Meetings.FirstOrDefault(m => m.Id == id);
            var dialog = new MeetingEditDialog(item.Clone());
            dialog.Events()
                .Closed
                .Subscribe(toplevel =>
                {
                    var dialog = (MeetingEditDialog)toplevel;
                    if (dialog.CloseType == DialogCloseType.Ok)
                    {
                        dialog.ViewModel.CopyTo(item);
                    }
                });

            Application.Run(dialog);
        };

        ViewModel.Meetings.Events()
            .CollectionChanged
            .Subscribe(args =>
            {
                if (args.OldItems != null)
                {
                    foreach (INotifyPropertyChanged item in args.OldItems)
                        item.PropertyChanged -= MeetingItemPropertyChanged;
                }
                if (args.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in args.NewItems)
                        item.PropertyChanged += MeetingItemPropertyChanged;
                }

                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    MeetingsList.Table.Clear();
                    MeetingsList.Update();

                    return;
                }

                foreach (var item in args.NewItems.Cast<MeetingViewModel>())
                {
                    MeetingsList.Table.Rows.Add(
                        item.Id,
                        item.Subject,
                        item.StartDateTime,
                        item.EndDateTime,
                        item.NotifyBeforeTime,
                        item.HasBeenNotifiedAbout
                    );
                }

                MeetingsList.Update();
            })
            .DisposeWith(_disposable);

        Add(MeetingsList);
    }

    private void MeetingItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var item = (MeetingViewModel)sender;
        var row = MeetingsList.Table.Rows.Find(item.Id);
        if (row == default)
        {
            return;
        }

        row[e.PropertyName] = item.GetType().GetProperty(e.PropertyName)?.GetValue(item);
    }

    private DataTable CreateMeetingsDataTable()
    {
        var table = new DataTable();

        var idColumn = new DataColumn()
        {
            Caption    = "Id",
            ColumnName = "Id",
            DataType = typeof(Guid),
        };

        var subjectColumn = new DataColumn()
        {
            Caption    = "Тема",
            ColumnName = "Subject",
            DataType   = typeof(string),
        };

        var startDate = new DataColumn()
        {
            Caption    = "Начало",
            ColumnName = "StartDateTime",
            DataType   = typeof(DateTime),
        };

        var endDate = new DataColumn()
        {
            Caption    = "Окончание",
            ColumnName = "EndDateTime",
            DataType   = typeof(DateTime),
        };

        var notifyBefore = new DataColumn()
        {
            Caption    = "Оповещение за",
            ColumnName = "NotifyBeforeTime",
            DataType   = typeof(TimeSpan),
        };

        var isNotified = new DataColumn()
        {
            Caption    = "Уведомление выслано",
            ColumnName = "HasBeenNotifiedAbout",
            DataType   = typeof(bool),
        };

        var columns = table.Columns;
        columns.Add(idColumn);
        columns.Add(subjectColumn);
        columns.Add(startDate);
        columns.Add(endDate);
        columns.Add(notifyBefore);
        columns.Add(isNotified);

        table.PrimaryKey = new[] { idColumn };

        return table;
    }

    protected override void Dispose(bool disposing)
    {
        _disposable.Dispose();
        base.Dispose(disposing);
    }
}
