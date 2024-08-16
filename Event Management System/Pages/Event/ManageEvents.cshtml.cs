using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Event_Management_System.Pages
{
    [Authorize(Roles = "Organizer")]
    public class ManageEventsModel : PageModel
    {
        private readonly IEventService _eventService;

        public IList<Event_Management_System.Models.Event> Events { get; set; }

        public ManageEventsModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task OnGetAsync()
        {
            Events = (await _eventService.GetAllEventsAsync()).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _eventService.DeleteEventAsync(id);
            return RedirectToPage();
        }
    }

}
