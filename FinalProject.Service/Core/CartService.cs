using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<CartViewModel>> GetUserCardViewModel(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var carts = await _context.Carts.Where(p => p.ApplicationUserId == userId).Include(c => c.Product).Include(c => c.Product.Favorites).ToListAsync();
            var cartViewModels = carts.Select(c => new CartViewModel
            {
                ProductId = c.ProductId,
                Product = c.Product,
                Quantity = c.Quantity
            }).ToList();
            return cartViewModels;
        }

        public async Task<Product> GetProductById(int productId)
        {
            Product? product = await _context.Products.FindAsync(productId);
            return product;
        }

        public async Task<bool> ProductExist(int productId)
        {
            Product? product = await _context.Products.FindAsync(productId);
            return product != null;
        }

        public async Task<CartViewModel> GetExistingCartItem(string? userId, int productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var existingCartItem = await _context.Carts.FirstOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == productId);
            if (existingCartItem != null) { 
                var existingCartViewModels = new CartViewModel
                {
                    ProductId = existingCartItem.ProductId,
                    Product = existingCartItem.Product,
                    Quantity = existingCartItem.Quantity
                };
                return existingCartViewModels;
            }
            return null;
        }

        public async Task AddCart(String userId, int productId)
        {
            Product? product = await GetProductById(productId);
            var user = await _userManager.FindByIdAsync(userId);
            var cartItem = new Cart
            {
                ApplicationUserId = userId,
                ApplicationUser = user,
                ProductId = product.Id,
                Product = product,
                Quantity = 1
            };
            _context.Carts.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(String userId, int productId)
        {
            var existingCartItem = await _context.Carts.FirstOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == productId);
            if (existingCartItem != null) {
                existingCartItem.Quantity++;
                _context.Carts.Update(existingCartItem);
                await _context.SaveChangesAsync(); 
            }
        }

        public async Task DecreaseQuantityAsync(String userId, int productId)
        {
            var existingCartItem = await _context.Carts.FirstOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == productId);
            (existingCartItem.Quantity)--;
            _context.Carts.Update(existingCartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(String userId, int productId)
        {
            var existingCartItem = await _context.Carts.FirstOrDefaultAsync(p => p.ApplicationUserId == userId && p.ProductId == productId);
            _context.Carts.Remove(existingCartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CartCountAsync(string? userId)
        {
            var cartItemCount = await _context.Carts
                .Where(c => c.ApplicationUserId == userId)
                .SumAsync(c => c.Quantity);
            return cartItemCount;
        }
    }
}
