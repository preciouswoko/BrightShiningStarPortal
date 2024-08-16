using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Event_Management_System.Pages.Event
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IEventService _eventService;

        public IList<Event_Management_System.Models.Event> UpcomingEvents { get; set; }
        public bool IsAdmin { get; set; }

        public DashboardModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task OnGetAsync()
        {
            IsAdmin = User.IsInRole("Admin");

            if (IsAdmin)
            {
                UpcomingEvents = (await _eventService.GetAllEventsAsync()).ToList();
            }
            else
            {
                UpcomingEvents = (await _eventService.GetAllEventsAsync())
                    .Where(e => e.Date >= DateTime.Now)
                    .ToList();
            }
        }
    }

}
