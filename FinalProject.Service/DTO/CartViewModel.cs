using System.ComponentModel.DataAnnotations;

namespace FinalProject.Service.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public required string ApplicationUserId { get; set; }
        public required ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
