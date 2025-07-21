using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


    }
}
