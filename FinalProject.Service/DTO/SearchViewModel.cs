
namespace FinalProject.Service.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<Product> Results { get; set; } = new List<Product>();
    }
}

