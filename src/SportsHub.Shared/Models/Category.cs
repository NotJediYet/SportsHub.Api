using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;
        public bool IsStatic { get; set; } = false;

    }
}
