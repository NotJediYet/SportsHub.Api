using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;
namespace SportsHub.Shared.Models
{
    public class CreateArticleModel
    {
        public Guid TeamId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public IFormFile ArticleImage { get; set; }
    }
}