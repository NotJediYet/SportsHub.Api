using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Bytes is required.")]
        public byte[] Bytes { get; set; }
        [Required(ErrorMessage = "Image name is required.")]
        public string ImageName { get; set; }
        [Required(ErrorMessage = "File extention is required.")]
        public string FileExtension { get; set; }
        [Required(ErrorMessage = "Image size is required.")]
        public decimal ImageSize { get; set; }
        [Required(ErrorMessage = "ArticleId is required.")]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public Image(byte[] bytes, string imageName, string fileExtension, decimal imageSize, Guid articleId)
        {
            Bytes = bytes;
            ImageName = imageName;
            FileExtension = fileExtension;
            ImageSize = imageSize;
            ArticleId = articleId;
        }
    }
}