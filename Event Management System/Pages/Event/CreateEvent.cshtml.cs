using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Event_Management_System.Pages.Event
{
    [Authorize(Roles = "Organizer")]
    public class CreateEventModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public Event_Management_System.Models.Event Event { get; set; }

        public CreateEventModel(IEventService eventService, UserManager<ApplicationUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        public string UserId { get; set; }

        public async Task OnGetAsync()
        {
            // Retrieve the user ID from the claims
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Alternatively, if you are using UserManager
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Get the user ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                // Set the OrganizerId on the Event object
                Event.OrganizerId = userId;

                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}

                await _eventService.CreateEventAsync(Event);
                return RedirectToPage("/Event/ManageEvents");
            }
            else
            {
                // Handle the case where the user ID is not found
                ModelState.AddModelError(string.Empty, "User not authenticated. Please log in.");
                return Page();
            }
        }

    }

}
