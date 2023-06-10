
namespace FinalProject.Service.Models
{
    public class CartViewModel
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
