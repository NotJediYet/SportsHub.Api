﻿using Microsoft.EntityFrameworkCore;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.DBContext
{
    public class SportsHubDbContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Subcategory> Subcategories => Set<Subcategory>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Article> Articles => Set<Article>();
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
            modelBuilder.Entity<Article>()
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(a => a.TeamId);
            modelBuilder.Entity<Article>()
                .Property(a => a.IsPublished)
                .HasDefaultValue(false);
            modelBuilder.Entity<Article>()
                .Property(a => a.IsShowComments)
                .HasDefaultValue(false);
        }
    }
}
