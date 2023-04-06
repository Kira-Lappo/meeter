using System.Data;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Meeter.Models;
using Meeter.TerminalGui.ViewModels;
using NStack;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using Terminal.Gui;

namespace Meeter.TerminalGui.Views;

public enum DialogCloseType
{
    None = 0,
    Ok = 1,
}

public class MeetingEditDialog : DialogFor<MeetingViewModel>
{
    private readonly CompositeDisposable _disposable = new();

    public MeetingEditDialog(MeetingViewModel viewModel)
    {
        ViewModel = viewModel;
        Initialize();
    }

    public DialogCloseType CloseType { get; set; }

    private void Initialize()
    {
        var subjectInput = new TextField(ViewModel.Subject);
        subjectInput
            .Events()
            .TextChanged
            .Select(old => subjectInput.Text)
            .DistinctUntilChanged()
            .BindTo(ViewModel, x => x.Subject)
            .DisposeWith(_disposable);

        var startDateField = new DateField(ViewModel.StartDateTime);
        startDateField
            .Events()
            .TextChanged
            .Select(old => subjectInput.Text.ToString())
            .DistinctUntilChanged()
            .BindTo(ViewModel, x => x.StartDateTime)
            .DisposeWith(_disposable);

        var endDateField = new DateField(ViewModel.EndDateTime);
        endDateField
            .Events()
            .TextChanged
            .Select(old => subjectInput.Text.ToString())
            .DistinctUntilChanged()
            .BindTo(ViewModel, x => x.EndDateTime)
            .DisposeWith(_disposable);

        var notifyBeforeField = new TimeField(ViewModel.NotifyBeforeTime);
        notifyBeforeField
            .Events()
            .TextChanged
            .Select(old => subjectInput.Text.ToString())
            .DistinctUntilChanged()
            .BindTo(ViewModel, x => x.NotifyBeforeTime)
            .DisposeWith(_disposable);

        var isNotifiedField = new CheckBox(ustring.Empty, ViewModel.HasBeenNotifiedAbout);
        isNotifiedField.Enabled = false;

        AddInputRow(0, "Тема",                subjectInput);
        AddInputRow(1, "Начало",              startDateField);
        AddInputRow(2, "Окончание",           endDateField);
        AddInputRow(3, "Уведомление за",      notifyBeforeField);
        AddInputRow(4, "Уведомление выслано", isNotifiedField);

        var saveButton = new Button();
        saveButton.Text = "Save";
        saveButton.Clicked += () =>
        {
            CloseType = DialogCloseType.Ok;
            RequestStop();
        };

        AddButton(saveButton);

        var cancelButton = new Button();
        cancelButton.Text    =  "Cancel";
        cancelButton.Clicked += RequestStop;

        AddButton(cancelButton);
    }

    private void AddInputRow(int rowIndex, string title, View inputField)
    {
        var label = new Label()
        {
            Y             = rowIndex,
            Width         = Dim.Percent(50),
            TextAlignment = TextAlignment.Right,
            Text          = title,
        };

        inputField.X             = Pos.Percent(50);
        inputField.Y             = rowIndex;
        inputField.Width         = Dim.Percent(50);
        inputField.TextAlignment = TextAlignment.Left;

        Add(label, inputField);
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
}
