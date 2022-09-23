using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class EditCategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsStatic { get; set; } = false;

        public bool IsHidden { get; set; } = false;

        public int OrderIndex { get; set; }
    }
}
