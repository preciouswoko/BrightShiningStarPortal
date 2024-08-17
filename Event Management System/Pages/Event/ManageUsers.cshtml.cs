using Event_Management_System.Interfaces;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_Management_System.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<UserViewModel> Users { get; set; }

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            Users = (IList<UserViewModel>)users.Select(async user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            }).ToList();
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
