using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Service.Data;
using FinalProject.Service.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartCountViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cartItemCount = await _context.Carts
                .Where(c => c.ApplicationUserId == userId)
                .SumAsync(c => c.Quantity);

            return View("Default", cartItemCount);
        }
    }
}
