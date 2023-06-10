
namespace FinalProject.Service.Models
{
    public class SearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<ProductViewModel> Results { get; set; } = new List<ProductViewModel>();
    }
}

