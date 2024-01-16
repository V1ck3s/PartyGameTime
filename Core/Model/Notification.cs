namespace PartyGameTime.Core.Model;

public class Notification
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Read { get; set; }
    public Action OnClickAction { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string TimeSinceStr
    {
        get
        {
            var difference = DateTime.Now - CreatedAt;

            if (difference.Days > 0)
                return difference.Days.ToString() + "day(s) ago";

            if (difference.Hours > 0)
                return difference.Hours.ToString() + "hour(s) ago";

            return difference.Minutes.ToString() + "min(s) ago";
        }
    }
}