using Meeter.Models;
using Meeter.Services;
using Meeter.Services.Stores;
using Terminal.Gui;

namespace Meeter.TerminalGui.ViewModels;

public class AppViewModel : ViewModel
{
    private readonly IMeetingStore _meetingStore;
    private readonly IMeetingService _meetingService;
    private readonly DummyDataGenerationService _dummyDataGenerationService;
    private List<Meeting> _meetings;
    private DateTime _selectedPeriodDateTime = DateTime.Today;

    public AppViewModel(IMeetingStore meetingStore,
        DummyDataGenerationService dummyDataGenerationService,
        IMeetingService meetingService)
    {
        _meetingStore               = meetingStore;
        _dummyDataGenerationService = dummyDataGenerationService;
        _meetingService        = meetingService;

        ReloadMeetingsCommand    = new Command(ReloadMeetings);
        GenerateDummyDataCommand = new Command(GenerateDummyData);
        ExitCommand = new Command(Exit);
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
        Meetings = _meetingService.GetAllByStartDate(SelectedPeriodDateTime).ToList();
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
}
