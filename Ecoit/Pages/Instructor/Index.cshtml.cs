using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecoit.Pages.Instructor
{
    public class IndexModel : PageModel
    {
        public bool displayInvalidAccountMessage = false;

        IConfiguration Configuration;
        public IndexModel(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IActionResult OnGet()
        {
            Console.WriteLine();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Instructor/Formations");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string email, string password, string ReturnUrl)
        {
            var authSection = Configuration.GetRequiredSection("Auth");

            string adminLogin = authSection["AdminLogin"];
            string adminPassword = authSection["AdminPassword"];

            if ((email == adminLogin) && (password == adminPassword))
            {
                displayInvalidAccountMessage = false;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new
                ClaimsPrincipal(claimsIdentity));
                return Redirect(ReturnUrl == null ? "/Instructor" : ReturnUrl);
            }
            displayInvalidAccountMessage = true;
            return Page();
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Instructor");
        }
    }
}
