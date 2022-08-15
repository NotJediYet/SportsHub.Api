using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class CreateImageModel
    {
        public byte[] Bytes { get; set; }
        public string FileExtension { get; set; }
        public decimal ImageSize { get; set; }
        public string ImageName { get; set; }
        public Guid ArticleId { get; set; }
    }
}