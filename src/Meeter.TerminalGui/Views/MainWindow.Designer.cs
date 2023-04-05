using System.Data;
using Meeter.Models;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public partial class MainWindow : Window
{
    private TableView _meetingsTable;
    private DataTable _meetingTableData;
    // private Dialog _meetingEditDialog;
    // private Dictionary<string, TextField> _meetingEditInputs = new();

    private void InitializeComponent()
    {
        Width              = Dim.Fill();
        Height             = Dim.Fill();
        X                  = 0;
        Y                  = 0;
        Modal              = false;
        Border.BorderStyle = BorderStyle.Single;
        TextAlignment      = TextAlignment.Left;
        ColorScheme        = Colors.ColorSchemes["Dialog"];

        _meetingsTable = new TableView () {
            X           = 0,
            Y           = 0,
            Width       = Dim.Fill(),
            Height      = Dim.Fill(),
            FullRowSelect = true,
        };

        _meetingTableData = new DataTable();

        var idColumn = new DataColumn()
        {
            ColumnName = "Id",
            Unique = true,
            ReadOnly = true,
        };

        var subjectColumn = new DataColumn()
        {
            Caption = "Тема",
            ColumnName = "Subject",
        };

        var startDate = new DataColumn()
        {
            Caption = "Начало",
            ColumnName = "StartDateTime",
            DataType = typeof(DateTime),
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

        var columns = _meetingTableData.Columns;
        columns.Add(idColumn);
        columns.Add(subjectColumn);
        columns.Add(startDate);
        columns.Add(endDate);
        columns.Add(notifyBefore);
        columns.Add(isNotified);

        _meetingsTable.Table = _meetingTableData;

        Add(_meetingsTable);
    }

    private static TextField CreateInputField(DataColumn column)
    {
        if (column.DataType == typeof(DateTime))
        {
            return new DateField();
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
            inputField.ReadOnly      = column.ReadOnly;
            inputField.Text          = dataRow[column]?.ToString();

            dialog.Add(label, inputField);
        }

        var saveButton = new Button();
        saveButton.Text    =  "Save";
        saveButton.Clicked += () =>
        {
            dialog.RequestStop();
        };

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
}
