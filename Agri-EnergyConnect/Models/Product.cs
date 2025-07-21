using System.ComponentModel.DataAnnotations.Schema;

namespace Agri_EnergyConnect.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public DateTime DateSupplied { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }
    }

}
