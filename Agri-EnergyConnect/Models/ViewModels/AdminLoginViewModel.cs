using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
