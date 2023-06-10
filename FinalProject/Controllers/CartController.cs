using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FinalProject.Service.Data;
using FinalProject.Service.Models;
using FinalProject.Service.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Runtime.Intrinsics.X86;
using System;
using Microsoft.CodeAnalysis;

namespace FinalProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly FavoriteService _favoriteService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CartService cartService,FavoriteService favoriteService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
            _favoriteService = favoriteService;
        }
        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var cartViewModels = await _cartService.GetUserCardViewModel(userId);
                return View(cartViewModels);
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            bool isProductExist = await _cartService.ProductExist(productId);
            var userId = _userManager.GetUserId(User);

            if (!isProductExist)
            {
                return NotFound();
            }

            try
            {
                CartViewModel? existingCartItem = await _cartService.GetExistingCartItem(userId, productId);

                if (existingCartItem != null)
                {
                    await _cartService.IncreaseQuantityAsync(userId, productId);
                }
                else
                    await _cartService.AddCart(userId, productId);

                TempData["success"] = "Ürün Sepete Eklendli!";
                    
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ProductId", ex.Message);
            }
            return RedirectToAction("Details", "Products", new { Id = productId });
        }

        [HttpGet]
        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, productId);
            if (item == null)
            {
                return NotFound();
            }
            if (item.Quantity == 10)
            {
                TempData["warning"] = "Ürün en fazla 10 adet olabilir!";
            }else
            {
                await _cartService.IncreaseQuantityAsync(userId, productId);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, productId);
            if (item == null)
            {
                return NotFound();
            } 
            if (item.Quantity == 1) {
                await _cartService.RemoveItemAsync(userId, productId);
                TempData["warning"] = "Ürün Sepetten Silindi!";
            }
            else if (item.Quantity > 1 ) {
                await _cartService.DecreaseQuantityAsync(userId, productId);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, productId);
            if (item == null)
            {
                return NotFound();
            }

            await _cartService.RemoveItemAsync(userId, productId);
            TempData["warning"] = "Ürün Sepetten Silindi!";
            return RedirectToAction("Index");
        }
    }
    
}
