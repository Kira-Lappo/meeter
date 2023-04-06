using System.Collections.Specialized;
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

        ViewModel.GenerateDummyDataCommand.Execute()
            .Wait();
    }

    public ListView MeetingsList { get; set; }

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

        MeetingsList = new ListView()
        {
            X             = 0,
            Y             = 0,
            Width         = Dim.Fill(),
            Height        = Dim.Fill(),
        };

        MeetingsList.OpenSelectedItem += args =>
        {
            var item = (MeetingViewModel)args.Value;
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
                if (args.Action == NotifyCollectionChangedAction.Reset)
                {
                    MeetingsList.Clear();
                    return;
                }

                MeetingsList.SetSource(ViewModel.Meetings);
            })
            .DisposeWith(_disposable);

        Add(MeetingsList);
    }

    private DataTable CreateMeetingsDataTable()
    {
        var table = new DataTable();

        var subjectColumn = new DataColumn()
        {
            Caption    = "Тема",
            ColumnName = "Subject",
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
        columns.Add(subjectColumn);
        columns.Add(startDate);
        columns.Add(endDate);
        columns.Add(notifyBefore);
        columns.Add(isNotified);

        return table;
    }

    protected override void Dispose(bool disposing)
    {
        _disposable.Dispose();
        base.Dispose(disposing);
    }
}
