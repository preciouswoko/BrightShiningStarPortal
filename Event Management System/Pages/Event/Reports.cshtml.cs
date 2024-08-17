using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Event
{
    public class ReportsModel : PageModel
    {
        private readonly IEventService _eventService;

        public ReportsModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public IList<Report> Reports { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch data for the report
            var events = await _eventService.GetAllEventsAsync();

            // Example logic for generating reports - this will depend on your actual reporting needs
            Reports = events.Select(e => new Report
            {
                Title = e.Title,
                Description = e.Description,
                Date = e.Date
            }).ToList();
        }
    }
}
