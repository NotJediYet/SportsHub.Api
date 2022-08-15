using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class CreateTeamModel
    {
        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public Guid SubcategoryId { get; set; }

        public IFormFile Logo { get; set; }
        public IFormFile TeamLogo { get;set; }
    }
}
