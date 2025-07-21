using System;
using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class ProjectComment
    {
        public int Id { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Comment cannot be empty.")]
        [StringLength(500)]
        public string Content { get; set; }

        [StringLength(100)]
        public string AuthorName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
