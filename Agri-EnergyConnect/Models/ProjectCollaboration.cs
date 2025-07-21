using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class ProjectCollaboration
    {
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string OwnerName { get; set; }

        public string ContactEmail { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}