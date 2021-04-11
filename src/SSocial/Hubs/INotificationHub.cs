using System.Threading.Tasks;

namespace SSocial.Hubs
{
    public interface INotificationHub
    {
        Task SendNotification(string userId);
    }
}