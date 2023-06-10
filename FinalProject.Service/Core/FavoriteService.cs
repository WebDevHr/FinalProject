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
    public class FavoriteService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoriteService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Favorite>> GetAll(string userId)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.ApplicationUserId == userId)
                .ToListAsync();
            return favorites;
        }

        public Favorite? GetFavorite(int productId, string userId)
        {
            return _context.Favorites.FirstOrDefault(f => f.ApplicationUserId == userId && f.ProductId == productId);
        }

        public async Task AddFavorite(Favorite fav)
        {
            _context.Favorites.Add(fav);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFavorite(Favorite fav)
        {
            _context.Favorites.Remove(fav);
            await _context.SaveChangesAsync();
        }
    }
}
