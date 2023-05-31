using System.ComponentModel.DataAnnotations;

namespace FinalProject.Service.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int Count { get; set; } = 0;

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
