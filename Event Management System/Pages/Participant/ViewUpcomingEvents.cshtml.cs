using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Event
{
    public class ViewUpcomingEventsModel : PageModel
    {
        private readonly IEventService _eventService;

        public ViewUpcomingEventsModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public IEnumerable<Event_Management_System.Models.Event> Events { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var allEvents = await _eventService.GetAllEventsAsync();
            Events = allEvents.Where(e => e.Date > DateTime.Now).ToList();

            return Page();
        }
    }
}
