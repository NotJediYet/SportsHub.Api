using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required(ErrorMessage = "Team location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subcategory id is required.")]
        public Guid SubcategoryId { get; set; }

        public Team(string name, Guid subcategoryId, string location)
        {
            Name = name;
            SubcategoryId = subcategoryId;
            Location = location;
        }
    }
}
