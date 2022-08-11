using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class Logo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Image data is required.")]
        public byte[] Bytes { get; set; }

        [Required(ErrorMessage = "Upload date is required.")]
        public DateTime UploadDate { get; set; }

        [Required(ErrorMessage = "File extension is required.")]
        public string FileExtension { get; set; }

        [Required(ErrorMessage = "Team id is required.")]
        public Guid TeamId { get; set; }

        public Logo(byte[] bytes, DateTime uploadDate, string fileExtension, Guid teamId)
        {
            Bytes = bytes;
            UploadDate = uploadDate;
            FileExtension = fileExtension;
            TeamId = teamId;
        }
    }
}
