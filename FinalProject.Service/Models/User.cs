using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Service.Models
{
    public class User
    {
        [Key]
        public int? Id { get; set; }
        [DisplayName("Ad")]
        [Required(ErrorMessage = "Ad zorunludur")]
        public string FirstName { get; set; } = string.Empty;
        [DisplayName("Soyad")]
        [Required(ErrorMessage = "Soyad zorunludur")]
        public string LastName { get; set; } = string.Empty;
        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        public string PhoneNumber { get; set; } = string.Empty;
        [DisplayName("E-posta Adresi")]
        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        public string Email { get; set; } = string.Empty;
        [DisplayName("Şifre")]
        [Required(ErrorMessage = "Şifre zorunludur")]
        public string Password { get; set; } = string.Empty;


    }
}
