using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Code), IsUnique = true)]
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Language name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language code is required.")]
        public string Code { get; set; } = string.Empty;

        public bool? IsDefault { get; set; } = false;

        public bool? IsHidden { get; set; } = true;

        public bool? IsAdded { get; set; } = false;
    }
}
