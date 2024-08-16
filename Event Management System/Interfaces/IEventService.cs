using Event_Management_System.Models;

namespace Event_Management_System.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event> GetEventByIdAsync(int id);
        Task CreateEventAsync(Event @event);
        Task UpdateEventAsync(Event @event);
        Task DeleteEventAsync(int id);
        Task<bool> RegisterForEventAsync(int eventId, string userId);
    }
}
