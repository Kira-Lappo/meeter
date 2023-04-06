using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Meeter.TerminalGui.ViewModels;

public class MeetingViewModel : ReactiveObject
{
    public Guid Id { get; set; }

    [Reactive]
    public string Subject { get; set; }

    [Reactive]
    public DateTime StartDateTime { get; set; }

    [Reactive]
    public DateTime EndDateTime { get; set; }

    [Reactive]
    public TimeSpan NotifyBeforeTime { get; set; }

    [Reactive]
    public bool HasBeenNotifiedAbout { get; set; }

    public MeetingViewModel Clone()
    {
        var clone = new MeetingViewModel();
        CopyTo(clone);
        return clone;
    }

    public void CopyTo(MeetingViewModel clone)
    {
        clone.Id                   = this.Id;
        clone.Subject              = this.Subject;
        clone.StartDateTime        = this.StartDateTime;
        clone.EndDateTime          = this.EndDateTime;
        clone.NotifyBeforeTime     = this.NotifyBeforeTime;
        clone.HasBeenNotifiedAbout = this.HasBeenNotifiedAbout;
    }
}
