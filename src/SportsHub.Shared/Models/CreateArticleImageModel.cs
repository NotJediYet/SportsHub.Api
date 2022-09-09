using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class CreateArticleImageModel
    {
        public byte[] Bytes { get; set; }
        public string FileExtension { get; set; }
        public Guid ArticleId { get; set; }
    }
}