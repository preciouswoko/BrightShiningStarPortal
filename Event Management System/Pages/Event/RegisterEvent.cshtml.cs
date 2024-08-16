using Event_Management_System.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Event_Management_System.Pages.Event
{
    [Authorize]
    public class RegisterEventModel : PageModel
    {
        private readonly IEventService _eventService;

        public Event_Management_System.Models.Event Event { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsFull { get; set; }

        public RegisterEventModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _eventService.GetEventByIdAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IsRegistered = Event.Registrations.Any(r => r.UserId == userId);
            IsFull = Event.Registrations.Count >= Event.MaxParticipants;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _eventService.RegisterForEventAsync(id, userId);

            if (!success)
            {
                return BadRequest("Unable to register for event.");
            }

            return RedirectToPage("Dashboard");
        }
    }

}
