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
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ProductService productService, CartService cartService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _cartService = cartService;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var carts = await _cartService.GetUserCard(userId);

                var cartViewModels = carts.Select(c => new CartViewModel
                {
                    Id = c.Id,
                    ApplicationUserId = c.ApplicationUserId,
                    ApplicationUser = user,
                    ProductId = c.ProductId,
                    Product = c.Product,
                    Quantity = c.Quantity
                }).ToList();
                return View(cartViewModels);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _cartService.GetProductById(productId);
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(userId);

                if (product == null)
                {
                    return NotFound();
                }

                var cartItem = new Cart
                {
                    ApplicationUserId = userId,
                    ApplicationUser = user,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1
                };

                try
                {
                    Cart? existingCartItem = await _cartService.GetExistingCartItem(userId, productId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity++;
                        await _cartService.UpdateCart(existingCartItem);
                    }
                    else
                        await _cartService.AddCart(cartItem);
                    
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("ProductId", ex.Message);
                }
            }
            return RedirectToAction("Details", "Products", new { Id = productId });
        }

        /////////
        ///
        [HttpGet]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, id);
            if (item == null)
            {
                return NotFound();
            }

            await _cartService.IncreaseQuantityAsync(item);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, id);
            if (item == null)
            {
                return NotFound();
            }

            await _cartService.DecreaseQuantityAsync(item);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, id);
            if (item == null)
            {
                return NotFound();
            }

            await _cartService.RemoveItemAsync(item);
            return RedirectToAction("Index");
        }
    }
    
}
