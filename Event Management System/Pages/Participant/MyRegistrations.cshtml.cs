using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Event_Management_System.Pages.Event
{
    public class MyRegistrationsModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyRegistrationsModel(IEventService eventService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        public IEnumerable<EventRegistration> Registrations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Event/Login");
            }

            var events = await _eventService.GetAllEventsAsync();
            Registrations = events
                .SelectMany(e => e.Registrations)
                .Where(r => r.UserId == user.Id)
                .ToList();

            return Page();
        }
    }
}
