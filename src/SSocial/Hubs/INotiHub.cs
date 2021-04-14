using System.Threading.Tasks;

namespace SSocial.Hubs
{
    public interface INotiHub
    {
        Task SendNotification(string title, string message);
    }
}