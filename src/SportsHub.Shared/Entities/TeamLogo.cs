using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class TeamLogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Image data is required.")]
        public byte[] Bytes { get; set; }

        [Required(ErrorMessage = "File extension is required.")]
        public string FileExtension { get; set; }

        [Required(ErrorMessage = "Image size is required.")]
        public decimal Size { get; set; }

        [Required(ErrorMessage = "Team id is required.")]
        public Guid TeamId { get; set; }

        public TeamLogo(byte[] bytes, string fileExtension, decimal size, Guid teamId)
        {
            Bytes = bytes;
            FileExtension = fileExtension;
            Size = size;
            TeamId = teamId;
        }
    }
}
