using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;


namespace Event_Management_System.Pages.Event.Dashboard
{

    [Authorize(Roles = "Participant")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
            // Initialization or data fetching for the Participant Dashboard
        }
    }

}
