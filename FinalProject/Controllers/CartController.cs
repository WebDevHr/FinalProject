﻿using Microsoft.AspNetCore.Mvc;
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

        public CartController(CartService cartService,FavoriteService favoriteService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _cartService = cartService;
            _userManager = userManager;
            _roleManager = roleManager;
            _favoriteService = favoriteService;
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

                //// Get the "Admin" role
                //var role = await _roleManager.FindByNameAsync("Admin");

                //// Check if the user is already in the "Admin" role
                //var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                //if (!isInRole)
                //{
                //    // Add the user to the "Admin" role
                //    await _userManager.AddToRoleAsync(user, "Admin");

                //    // Alternatively, you can add a new row to the AspNetUserRoles table
                //    // to map the user to the "Admin" role
                //    var userRole = new IdentityUserRole<string>
                //    {
                //        UserId = user.Id,
                //        RoleId = role.Id
                //    };
                //    await _cartService.AddAdmin(userRole);
                //}

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

                    TempData["success"] = "Ürün Sepete Eklendli!";
                    
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("ProductId", ex.Message);
                }
            }
            return RedirectToAction("Details", "Products", new { Id = productId });
        }

        [HttpGet]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var userId = _userManager.GetUserId(User);
            var item = await _cartService.GetExistingCartItem(userId, id);
            if (item == null)
            {
                return NotFound();
            }
            if (item.Quantity == 10)
            {
                TempData["warning"] = "Ürün en fazla 10 adet olabilir!";
            }else
            {
                await _cartService.IncreaseQuantityAsync(item);
            }
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
            if (item.Quantity == 1) {
                TempData["warning"] = "Ürün Sepetten Silindi!";
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
            TempData["warning"] = "Ürün Sepetten Silindi!";
            return RedirectToAction("Index");
        }
    }
    
}
