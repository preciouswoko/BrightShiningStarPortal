using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Event
{
    public class RegisterForEventModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterForEventModel(IEventService eventService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        public IEnumerable<Event_Management_System.Models.Event> Events { get; set; }
        public int SelectedEventId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Event/Login");
            }

            // Get all events that are not yet full
            Events = (await _eventService.GetAllEventsAsync())
                .Where(e => e.Registrations.Count < e.MaxParticipants);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Event/Login");
            }

            var success = await _eventService.RegisterForEventAsync(eventId, user.Id);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Unable to register for the event. It might be full or you might be already registered.");
            }

            return RedirectToPage("/Event/ManageEvents");
        }
    }
}
