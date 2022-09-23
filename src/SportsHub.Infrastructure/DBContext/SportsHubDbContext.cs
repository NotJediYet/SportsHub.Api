using Microsoft.EntityFrameworkCore;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.DBContext
{
    public class SportsHubDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamLogo> TeamLogos => Set<TeamLogo>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<ArticleImage> Images => Set<ArticleImage>();
        public DbSet<Language> Languages => Set<Language>();

        public SportsHubDbContext(DbContextOptions<SportsHubDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("CategoryOrderIndexes");
            modelBuilder.HasSequence<int>("SubcategoryOrderIndexes");
            modelBuilder.HasSequence<int>("TeamOrderIndexes");

            modelBuilder.Entity<Category>()
                .Property(category => category.IsStatic)
                .HasDefaultValue(false);
            modelBuilder.Entity<Category>()
                .Property(category => category.IsHidden)
                .HasDefaultValue(false);
            modelBuilder.Entity<Category>()
                .Property(category => category.OrderIndex)
                .HasDefaultValueSql("NEXT VALUE FOR CategoryOrderIndexes");
            modelBuilder.Entity<Subcategory>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(subcategory => subcategory.CategoryId);
            modelBuilder.Entity<Subcategory>()
                .Property(subcategory => subcategory.IsHidden)
                .HasDefaultValue(false);
            modelBuilder.Entity<Subcategory>()
                .Property(subcategory => subcategory.OrderIndex)
                .HasDefaultValueSql("NEXT VALUE FOR SubcategoryOrderIndexes");
            modelBuilder.Entity<Team>()
                .HasOne<Subcategory>()
                .WithMany()
                .HasForeignKey(team => team.SubcategoryId);
            modelBuilder.Entity<Team>()
                .Property(team => team.IsHidden)
                .HasDefaultValue(false);
            modelBuilder.Entity<Team>()
                .Property(team => team.OrderIndex)
                .HasDefaultValueSql("NEXT VALUE FOR TeamOrderIndexes");
            modelBuilder.Entity<TeamLogo>()
                .HasOne<Team>()
                .WithOne()
                .HasForeignKey<TeamLogo>(teamLogo => teamLogo.TeamId);
            modelBuilder.Entity<Article>()
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(article => article.TeamId);
            modelBuilder.Entity<Article>()
                .Property(article => article.IsPublished)
                .HasDefaultValue(false);
            modelBuilder.Entity<Article>()
                .Property(article => article.IsShowComments)
                .HasDefaultValue(false);
            modelBuilder.Entity<ArticleImage>()
                .HasOne<Article>()
                .WithOne()
                .HasForeignKey<ArticleImage>(articleImage => articleImage.ArticleId);
            modelBuilder.Entity<Language>()
                .Property(language => language.IsDefault)
                .HasDefaultValue(false);
            modelBuilder.Entity<Language>()
                .Property(language => language.IsHidden)
                .HasDefaultValue(true);
            modelBuilder.Entity<Language>()
                .Property(language => language.IsAdded)
                .HasDefaultValue(false);
        }
    }
}