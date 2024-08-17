using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Admin
{
    public class EditUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditUserModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public ApplicationUser User { get; set; }

        [BindProperty]
        public string Role { get; set; }

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

            // Fetch the user's role (assuming one role per user)
            var roles = await _userManager.GetRolesAsync(User);
            Role = roles.Count > 0 ? roles[0] : "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(User.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = User.UserName;
            user.Email = User.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Update the user's role
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, Role);

                return RedirectToPage("/Event/ManageUsers");
            }

            // Handle errors here
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
