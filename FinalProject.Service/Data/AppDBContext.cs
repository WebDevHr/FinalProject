using FinalProject.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Service.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;

        public DbSet<Product> Product { get; set; } = default!;

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

    }
}
