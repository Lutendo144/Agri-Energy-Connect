using System;
using System.ComponentModel.DataAnnotations;

namespace Agri_EnergyConnect.Models
{
    public class MarketplaceItem
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string SellerName { get; set; }

        public string SellerEmail { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
