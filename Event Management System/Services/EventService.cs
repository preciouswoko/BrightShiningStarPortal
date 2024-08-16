namespace Event_Management_System.Services
{
    using Event_Management_System.Data;
    using Event_Management_System.Interfaces;
    using Event_Management_System.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public EventService(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.Include(e => e.Registrations).ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _context.Events.Include(e => e.Registrations)
                                         .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateEventAsync(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event @event)
        {
            _context.Entry(@event).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> RegisterForEventAsync(int eventId, string userId)
        {
            var @event = await GetEventByIdAsync(eventId);
            if (@event == null || @event.Registrations.Count >= @event.MaxParticipants)
            {
                return false;
            }

            if (@event.Registrations.Any(r => r.UserId == userId))
            {
                return false;
            }

            var registration = new EventRegistration
            {
                EventId = eventId,
                UserId = userId
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            // Send email notification
            var user = await _context.Users.FindAsync(userId);
            await _emailSender.SendEmailAsync(user.Email, "Event Registration", $"You have successfully registered for the event '{@event.Title}'.");

            return true;
        }
    }

}
