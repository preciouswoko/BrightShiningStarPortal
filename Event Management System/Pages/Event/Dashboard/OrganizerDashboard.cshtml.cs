using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Event_Management_System.Pages.Event
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerDashboardModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizerDashboardModel(IEventService eventService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        // Property with both get and set accessors
        public IEnumerable<Event_Management_System.Models.Event> Events { get; set; } =  new List<Event_Management_System.Models.Event>();

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    Events = (await _eventService.GetAllEventsAsync())
                        .Where(e => e.OrganizerId == user.Id);
                }
                else
                {
                    // Handle the case where the user is null, even though User is authenticated
                    ModelState.AddModelError(string.Empty, "User not found. Please ensure you are logged in.");
                }
            }
            else
            {
                // Handle the case where the user is not authenticated
                ModelState.AddModelError(string.Empty, "User is not authenticated. Please log in.");
            }
        }

    }
}
