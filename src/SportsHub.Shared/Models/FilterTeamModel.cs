using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class FilterTeamModel
    {
        public string Location { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Subcategory { get; set; } = string.Empty;
    }
}
