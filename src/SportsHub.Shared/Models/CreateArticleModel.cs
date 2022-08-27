using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;
namespace SportsHub.Shared.Models
{
    public class CreateArticleModel
    {
        public Guid TeamId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string AltImage { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsShowComments { get; set; } = false;
        public IFormFile ArticleImage { get; set; }
    }
}