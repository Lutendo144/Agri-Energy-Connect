using System;
using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class ProjectJoin
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string JoinerName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string JoinerEmail { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
