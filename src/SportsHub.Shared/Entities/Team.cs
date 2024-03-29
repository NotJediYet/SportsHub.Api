﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string CreationDate { get; set; }

        [Required(ErrorMessage = "Subcategory id is required.")]
        public Guid SubcategoryId { get; set; }

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }

        [FromForm]
        [NotMapped]
        public TeamLogo TeamLogo { get; set; }
    }
}
