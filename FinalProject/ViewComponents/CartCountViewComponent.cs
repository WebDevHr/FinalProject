using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Service.Data;
using FinalProject.Service.Models;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Service.Core;

namespace FinalProject.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartCountViewComponent(CartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cartItemCount = await _cartService.CartCountAsync(userId);

            return View("Default", cartItemCount);
        }
    }
}
