using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SportsHub.Web.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Team
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Team name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Subcategory id is required.")]
        public int SubcategoryId { get; set; }
        
        public Team(string name, int subcategoryId)
        {
            Name = name;
            SubcategoryId = subcategoryId;
        }
    }
}
