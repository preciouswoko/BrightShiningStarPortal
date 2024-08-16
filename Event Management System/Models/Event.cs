namespace Event_Management_System.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; }
        public int MaxParticipants { get; set; }
        public ICollection<EventRegistration> Registrations { get; set; }
    }
}
