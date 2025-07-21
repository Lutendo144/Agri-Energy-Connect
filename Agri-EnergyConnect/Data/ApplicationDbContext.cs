using Microsoft.EntityFrameworkCore;
using Agri_EnergyConnect.Models;

namespace Agri_EnergyConnect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MarketplaceItem> MarketplaceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<MarketplaceItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)"); 

           
        }

        public DbSet<ProjectCollaboration> ProjectCollaborations { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }
        public DbSet<ProjectJoin> ProjectJoins { get; set; }

    }
}

