using System.ComponentModel;
using System.Runtime.CompilerServices;
using Meeter.Models;
using Meeter.Services;
using Meeter.Services.Stores;
using Terminal.Gui;

namespace Meeter.TerminalGui.ViewModels;

public class AppViewModel : IViewModel
{
    private readonly IMeetingStore _meetingStore;
    private readonly DummyDataGenerationService _dummyDataGenerationService;
    private List<Meeting> _meetings;
    private DateTime _selectedPeriodDateTime = DateTime.Today;

    private readonly Dictionary<string, Func<object>> _properties;

    public AppViewModel(IMeetingStore meetingStore,
        DummyDataGenerationService dummyDataGenerationService)
    {
        _meetingStore               = meetingStore;
        _dummyDataGenerationService = dummyDataGenerationService;

        ReloadMeetingsCommand    = new Command(ReloadMeetings);
        GenerateDummyDataCommand = new Command(GenerateDummyData);
        ExitCommand = new Command(Exit);

        _properties = new()
        {
            {nameof(Meetings), () => Meetings},
            {nameof(SelectedPeriodDateTime), () => SelectedPeriodDateTime},
            {nameof(ReloadMeetingsCommand), () => ReloadMeetingsCommand},
            {nameof(GenerateDummyDataCommand), () => GenerateDummyDataCommand},
            {nameof(ExitCommand), () => ExitCommand},
        };
    }

    public List<Meeting> Meetings
    {
        get => _meetings;
        set
        {
            if (Equals(value, _meetings))
                return;

            SetField(ref _meetings, value);
        }
    }

    public DateTime SelectedPeriodDateTime
    {
        get => _selectedPeriodDateTime;
        set
        {
            if (value.Equals(_selectedPeriodDateTime))
                return;

            SetField(ref _selectedPeriodDateTime, value);
        }
    }

    public ICommand ReloadMeetingsCommand { get; }

    public ICommand GenerateDummyDataCommand { get; }

    public ICommand ExitCommand { get; }

    private void ReloadMeetings()
    {
        Meetings = _meetingStore.GetAll().ToList();
    }

    private void GenerateDummyData()
    {
        _dummyDataGenerationService.Generate();
        ReloadMeetings();
    }

    private void Exit()
    {
        Application.RequestStop();
    }

    #region INotifyPropertyChanged

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);

        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion INotifyPropertyChanged

    #region IPropertyBag

    public object this[string name]
    {
        get
        {
            if (_properties.TryGetValue(name, out var getValue))
            {
                return getValue();
            }

            return default;
        }
    }

    #endregion IPropertyBag
}
