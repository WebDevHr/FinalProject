using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Service.Core
{
    public class FavoriteService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<FavoriteViewModel>> GetAll(string userId)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.ApplicationUserId == userId)
                .ToListAsync();
            var favoriteViewModel = favorites.Select(c => new FavoriteViewModel 
            { 
                Product = c.Product
            }).ToList();
            return favoriteViewModel;
        }

        public bool IsFavorite(int productId, string userId)
        {
            return _context.Favorites.FirstOrDefault(f => f.ApplicationUserId == userId && f.ProductId == productId) != null;
        }

        public async Task AddFavorite(int productId, string userId)
        {
            var favorite = new Favorite
            {
                ApplicationUserId = userId,
                ProductId = productId
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFavorite(int productId, string userId)
        {
            var favorite = _context.Favorites.FirstOrDefault(f => f.ApplicationUserId == userId && f.ProductId == productId);
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
