using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Service.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("Ad")]
        [Required(ErrorMessage = "Ad zorunludur")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Soyad")]
        [Required(ErrorMessage = "Soyad zorunludur")]
        public string LastName { get; set; } = string.Empty;

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
