using Microsoft.EntityFrameworkCore;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.DBContext
{
    public class SportsHubDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();
        public DbSet<Team> Teams => Set<Team>();

        public SportsHubDbContext(DbContextOptions<SportsHubDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.IsStatic)
                .HasDefaultValue(false);
            modelBuilder.Entity<Subcategory>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(s => s.CategoryId);
            modelBuilder.Entity<Team>()
               .HasOne<Subcategory>()
               .WithMany()
               .HasForeignKey(t => t.SubcategoryId);

        }
    }
}
