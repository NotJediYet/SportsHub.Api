using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Subcategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Subcategory name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category id is required.")]
        public Guid CategoryId { get; set; }

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }
    }
}