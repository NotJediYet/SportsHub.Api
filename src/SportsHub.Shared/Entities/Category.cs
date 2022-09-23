using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;

        public bool IsStatic { get; set; } = false;

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }
    }
}
