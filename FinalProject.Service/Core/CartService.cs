using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Service.Core
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddToCart(Cart cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetUserCard(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var carts = await _context.Carts.Where(p => p.ApplicationUserId == userId).Include(c => c.Product).ToListAsync();
            return carts;
        }

        public async Task<Product> GetProductById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product;
        }

        public async Task<Cart> GetExistingCartItem(string userId, int productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var existingCartItem = await _context.Carts.FirstOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == productId);
            return existingCartItem;
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(Cart cart)
        {
            (cart.Quantity)++;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(Cart cart)
        {
            if (cart.Quantity > 1 )
            {
                (cart.Quantity)--;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveItemAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task AddAdmin(IdentityUserRole<string> userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
