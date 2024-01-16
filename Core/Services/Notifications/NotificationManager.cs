using PartyGameTime.Core.Model;

namespace PartyGameTime.Core.Services.Notifications;

public class NotificationManager
{
    public NotificationManager()
    {
        
    }

    public List<Notification> Notifications { get; set; } = new List<Notification>();
    public event Action<Notification> OnNewNotification;
    public event Action OnNotificationRead;

    public void SendNotification(Notification notification)
    {
        Notifications.Add(notification);
        OnNewNotification?.Invoke(notification);
    }

    public void SendNotification(string title, string content)
    {
        var notification = new Notification
        {
            Content = content,
            Title = title,
            CreatedAt = DateTime.Now
        };
        SendNotification(notification);
    }

    public async Task<List<Notification>> GetNotifications()
    {
        return Notifications;
    }

    public void MarkNotificationAsRead(Notification notification)
    {
        var notif = Notifications.FirstOrDefault(x => x.Id == notification.Id);
        if (notif is not null)
        {
            notif.Read = true;
            OnNotificationRead?.Invoke();
        }
    }
    public void MarkNotificationAllAsRead()
    {
        Notifications.ForEach(x => x.Read = true);
        OnNotificationRead?.Invoke();
    }

    public bool NotificationsUnread()
    {
        return Notifications.Any(x => x.Read == false);
    }
    
    
}