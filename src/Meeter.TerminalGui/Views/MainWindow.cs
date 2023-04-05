using System.Data;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Meeter.TerminalGui.ViewModels;
using ReactiveUI;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public class MainWindow : Window, IViewFor<MainWindowViewModel>
{
    private readonly CompositeDisposable _disposable = new();

    public MainWindow(MainWindowViewModel viewModel)
    {
        ViewModel = viewModel;
        Initialize();

        ViewModel.GenerateDummyDataCommand.Execute()
            .Wait();
    }

    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = value as MainWindowViewModel
                           ?? throw new ArgumentException("Should be of AppViewModel type", nameof(value));
    }

    public MainWindowViewModel ViewModel { get; set; }

    public TableView MeetingsTable { get; set; }

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

        MeetingsTable = new TableView()
        {
            X             = 0,
            Y             = 0,
            Width         = Dim.Fill(),
            Height        = Dim.Fill(),
            FullRowSelect = true,
        };

        this.OneWayBind(ViewModel,
                vm => vm.Meetings,
                v => v.MeetingsTable.Table)
            .DisposeWith(_disposable);

        var scrollView = new ScrollView()
        {
            AutoHideScrollBars            = true,
            ShowHorizontalScrollIndicator = false,
            ShowVerticalScrollIndicator   = true,
        };

        scrollView.Add(MeetingsTable);

        Add(scrollView);
    }

    private static View CreateInputField(DataColumn column)
    {
        if (column.DataType == typeof(DateTime))
        {
            return new DateField();
        }

        if (column.DataType == typeof(bool))
        {
            return new CheckBox();
        }

        if (column.DataType == typeof(TimeSpan))
        {
            return new TimeField();
        }

        return new TextField();
    }

    private Dialog CreateMeetingEditDialog(DataRow dataRow)
    {
        var dialog = new Dialog();
        var columns = dataRow.Table.Columns
            .Cast<DataColumn>()
            .Indexed();

        foreach (var (column, i) in columns)
        {
            var title = column.Caption;
            var label = new Label()
            {
                Y             = i,
                Width         = Dim.Percent(50),
                TextAlignment = TextAlignment.Right,
                Text          = title,
            };

            var inputField = CreateInputField(column);

            inputField.Id            = CreateInputId(column.ColumnName);
            inputField.X             = Pos.Percent(50);
            inputField.Y             = i;
            inputField.Width         = Dim.Percent(50);
            inputField.TextAlignment = TextAlignment.Left;
            inputField.Text          = dataRow[column]?.ToString();

            if (inputField is TextField textField)
            {
                textField.ReadOnly = column.ReadOnly;
            }

            dialog.Add(label, inputField);
        }

        var saveButton = new Button();
        saveButton.Text    =  "Save";
        saveButton.Clicked += () => { dialog.RequestStop(); };

        dialog.AddButton(saveButton);

        var cancelButton = new Button();
        cancelButton.Text    =  "Cancel";
        cancelButton.Clicked += () => { dialog.RequestStop(); };

        dialog.AddButton(cancelButton);

        return dialog;
    }

    private static string CreateInputId(string columnName)
    {
        return $"edit_meeting_{columnName}";
    }

    protected override void Dispose(bool disposing)
    {
        _disposable.Dispose();
        base.Dispose(disposing);
    }
}
