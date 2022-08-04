﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Subcategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Subcategory name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category id is required.")]
        public Guid CategoryId { get; set; }

        public Subcategory(string name, Guid categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }
    }
}