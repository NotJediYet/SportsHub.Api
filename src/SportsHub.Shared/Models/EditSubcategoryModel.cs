using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class EditSubcategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }
    }
}
