using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SportsHub.Web.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Subcategory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Subcategory name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category id is required.")]
        public int CategoryId { get; set; }

        public Subcategory(string name, int categoryId)
        {
            Name = name;
            CategoryId = categoryId;
        }
    }
}
