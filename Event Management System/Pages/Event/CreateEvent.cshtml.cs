using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Event_Management_System.Pages.Event
{
    [Authorize(Roles = "Organizer")]
    public class CreateEventModel : PageModel
    {
        private readonly IEventService _eventService;

        [BindProperty]
        public Event_Management_System.Models.Event Event { get; set; }

        public CreateEventModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _eventService.CreateEventAsync(Event);
            return RedirectToPage("ManageEvents");
        }
    }

}
