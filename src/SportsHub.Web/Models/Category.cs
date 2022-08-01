using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SportsHub.Web.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
        public bool IsStatic { get; set; } = false;

        public Category(string name)
        {
            Name = name;
        }
    }
}
