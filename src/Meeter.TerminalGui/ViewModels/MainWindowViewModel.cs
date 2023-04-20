using System.Collections.ObjectModel;
using System.Data;
using System.Reactive;
using DynamicData;
using Meeter.Models;
using Meeter.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Terminal.Gui;

namespace Meeter.TerminalGui.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly IMeetingService _meetingService;
    private readonly DummyDataGenerationService _dummyDataGenerationService;

    public MainWindowViewModel(DummyDataGenerationService dummyDataGenerationService,
        IMeetingService meetingService)
    {
        _dummyDataGenerationService = dummyDataGenerationService;
        _meetingService             = meetingService;

        GenerateDummyData = ReactiveCommand.Create(GenerateDummyDataMethod);
    }

    [Reactive]
    public ObservableCollection<MeetingViewModel> Meetings { get; set; } = new();

    [Reactive]
    public DateTime SelectedPeriodDateTime { get; set; } = DateTime.Today;

    public ReactiveCommand<Unit, Unit> GenerateDummyData { get; }

    private void GenerateDummyDataMethod()
    {
        _dummyDataGenerationService.Generate();
        ReloadMeetings();
    }

    private void ReloadMeetings()
    {
        var meetings = _meetingService.GetAllByStartDate(SelectedPeriodDateTime).ToList();
        var meetingsViewModels = meetings.Select(MapViewModel);

        Meetings.Clear();
        Meetings.Add(meetingsViewModels);
    }

    private MeetingViewModel MapViewModel(Meeting m)
    {
        return new MeetingViewModel
        {
            Id                   = m.Id,
            Subject              = m.Subject,
            StartDateTime        = m.StartDateTime,
            EndDateTime          = m.EndDateTime,
            NotifyBeforeTime     = m.NotifyBeforeTime,
            HasBeenNotifiedAbout = m.HasBeenNotifiedAbout,
        };
    }
}
