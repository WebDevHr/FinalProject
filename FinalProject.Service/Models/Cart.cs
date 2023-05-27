using System.ComponentModel.DataAnnotations;

namespace FinalProject.Service.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public required string ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
