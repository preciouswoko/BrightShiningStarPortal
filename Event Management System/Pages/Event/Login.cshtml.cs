using Event_Management_System.Models;
using Event_Management_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Event_Management_System.Pages.Event
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenUtility _tokenUtility;
        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, TokenUtility tokenUtility)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenUtility = tokenUtility;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    // Generate the JWT token
                    var token = _tokenUtility.GenerateToken(user);

                    // Store the token in a cookie or return it to the client
                    HttpContext.Response.Cookies.Append("JwtToken", token);

                    // Manually create the identity and sign in using Cookie Authentication
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

                    var roles = await _userManager.GetRolesAsync(user);
                    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                    var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in the user with the cookie scheme
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                    // Determine the appropriate redirect based on the user's role
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToPage("/Event/Dashboard/AdminDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Organizer"))
                    {
                        return RedirectToPage("/Event/Dashboard/OrganizerDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Participant"))
                    {
                        return RedirectToPage("/Event/Dashboard/Dashboard");
                    }
                    else
                    {
                        return RedirectToPage("/Index"); // Default redirection for unknown roles
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }



    }


}
