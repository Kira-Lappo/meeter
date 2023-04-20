using Meeter.Models;

namespace Meeter.Services.Notifications;

public class UWPNotificationSender : INotificationSender
{
    public void SendNotification(Meeting meeting)
    {
        // Todo [2023/04/20 kira] resolve notification issue
        // new ToastContentBuilder()
        //     .AddArgument("action",         "viewConversation")
        //     .AddArgument("conversationId", ConversationId)
        //     .AddText(meeting.Subject)
        //     .AddText($"Начало в {GetLocalTimeString(meeting.StartDateTime)}")
        //     .AddText($"Окончание в {GetLocalTimeString(meeting.EndDateTime)}")
        //     .Show();
    }
}
