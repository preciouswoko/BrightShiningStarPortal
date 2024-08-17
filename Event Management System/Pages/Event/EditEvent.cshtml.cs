using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Event
{
    [Authorize(Roles = "Organizer, Admin")]
    public class EditEventModel : PageModel
    {
        private readonly IEventService _eventService;

        public EditEventModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        [BindProperty]
        public Event_Management_System.Models.Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _eventService.GetEventByIdAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _eventService.UpdateEventAsync(Event);
            return RedirectToPage("/Event/ManageEvents");
        }
    }
}
