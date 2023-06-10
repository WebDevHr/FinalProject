// Controllers/FavoritesController.cs
using FinalProject.Service.Core;
using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly FavoriteService _favoriteService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(FavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _favoriteService = favoriteService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var favoriteViewModels = await _favoriteService.GetAll(userId);
            return View(favoriteViewModels);
        }
        [Authorize]
        public async Task<IActionResult> ToggleFavorite(int productId)
        {
            var userId = _userManager.GetUserId(User);
            bool isFavorite = _favoriteService.IsFavorite(productId, userId);

            if (!isFavorite)
                await _favoriteService.AddFavorite(productId, userId);
            else
                await _favoriteService.RemoveFavorite(productId, userId);
            

            // Get the referrer URL and redirect back to the same page
            var referrerUrl = Request.Headers["Referer"].ToString();
            return Redirect(referrerUrl);
        }
    }
}
