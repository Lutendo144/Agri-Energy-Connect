using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 
    }
}
