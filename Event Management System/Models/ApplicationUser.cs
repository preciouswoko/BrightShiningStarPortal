using Microsoft.AspNetCore.Identity;

namespace Event_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
       // public string Role { get; set; }
    }
    public class ApplicationUser1 : IdentityUser
    {
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
