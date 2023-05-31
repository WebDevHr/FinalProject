using Microsoft.AspNetCore.Http;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalProject.Filters
{
    public class IsAdminFilter : IAsyncActionFilter
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IsAdminFilter(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_signInManager.IsSignedIn(context.HttpContext.User))
            {
                var user = await _userManager.GetUserAsync(context.HttpContext.User);
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin) 
                    context.HttpContext.Session.SetInt32("IsAdmin", 1);
                else
                    context.HttpContext.Session.SetInt32("IsAdmin", 0);

            }

            await next();
        }
    }

}
