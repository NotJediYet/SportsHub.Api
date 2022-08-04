using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Team name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subcategory id is required.")]
        public Guid SubcategoryId { get; set; }

        public Team(string name, Guid subcategoryId)
        {
            Name = name;
            SubcategoryId = subcategoryId;
        }
    }
}
