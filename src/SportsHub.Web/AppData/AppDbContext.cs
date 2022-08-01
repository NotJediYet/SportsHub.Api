using Microsoft.EntityFrameworkCore;
using SportsHub.Web.Models;

namespace SportsHub.Web.AppData 
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Subcategory>? Subcategories { get; set; }
        public DbSet<Team>? Teams { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
