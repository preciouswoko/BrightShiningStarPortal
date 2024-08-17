using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Admin
{
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ApplicationUser User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            User = await _userManager.FindByIdAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(User);

            if (result.Succeeded)
            {
                return RedirectToPage("/Event/ManageUsers");
            }

            // Handle errors here
            ModelState.AddModelError(string.Empty, "Error deleting user.");
            return Page();
        }
    }
}
