using Microsoft.Extensions.Logging;

namespace Event_Management_System.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
    }
}
