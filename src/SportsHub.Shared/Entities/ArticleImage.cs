using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class ArticleImage
    {
        [Key]
        public byte[] Bytes { get; set; }

        [Required(ErrorMessage = "File extension name is required.")]
        public string FileExtension { get; set; }

        [Required(ErrorMessage = "ArticleImage size is required.")]
        public decimal ImageSize { get; set; }

        [Required(ErrorMessage = "ArticleId is required.")]
        public Guid ArticleId { get; set; }
        public ArticleImage(byte[] bytes, string fileExtension, decimal imageSize, Guid articleId)
        {
            Bytes = bytes;
            FileExtension = fileExtension;
            ImageSize = imageSize;
            ArticleId = articleId;
        }
    }
}