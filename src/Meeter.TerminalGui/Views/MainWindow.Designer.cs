using System.Data;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public partial class MainWindow : Window
{
    private TableView _meetingsTable;
    private DataTable _meetingTableData;
    private LineView _selectedDayLabel;

    private void InitializeComponent()
    {
        Width                  = Dim.Fill();
        Height                 = Dim.Fill();
        X                      = 0;
        Y                      = 0;
        Modal                  = false;
        Border.BorderStyle     = BorderStyle.None;
        TextAlignment          = TextAlignment.Left;

        _selectedDayLabel = new LineView(){
            X      = 0,
            Y      = 0,
            Width  = Dim.Fill(),
            Height = 1,
        };

        Add(_selectedDayLabel);

        _meetingsTable = new TableView () {
            X      = 0,
            Y      = 0,
            Width  = Dim.Fill(),
            Height = Dim.Fill(),
        };

        _meetingTableData = new DataTable();
        _meetingTableData.Columns.Add("Id");
        _meetingTableData.Columns.Add("Тема");
        _meetingTableData.Columns.Add("Начало");
        _meetingTableData.Columns.Add("Окончание");
        _meetingTableData.Columns.Add("Оповещение за");
        _meetingTableData.Columns.Add("Уведомление выслано");


        _meetingsTable.Table = _meetingTableData;

        Add(_meetingsTable);
    }
}
