using System;
using System.Data;
using Terminal.Gui;

namespace Meeter.Cli.Views;

public partial class MainWindow : Window
{
    private TableView _meetingsTable;

    private void InitializeComponent()
    {
        Width                  = Dim.Fill(0);
        Height                 = Dim.Fill(0);
        X                      = 0;
        Y                      = 0;
        Modal                  = false;
        Border.BorderStyle     = BorderStyle.None;
        TextAlignment          = TextAlignment.Left;

        _meetingsTable = new TableView () {
            X      = 0,
            Y      = 0,
            Width  = Dim.Fill(),
            Height = Dim.Fill(),
        };

        var dt = new DataTable();
        dt.Columns.Add("Тема");
        dt.Columns.Add("Начало");
        dt.Columns.Add("Окончание");
        dt.Columns.Add("Оповещение за");
        dt.Columns.Add("Уведомление выслано");

        _meetingsTable.Table = dt;

        Add(_meetingsTable);
    }
}
