using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    public class TeamLogo
    {
        [Key]
        [Required(ErrorMessage = "Team id is required.")]
        public Guid TeamId { get; set; }

        [Required(ErrorMessage = "Image data is required.")]
        public byte[] Bytes { get; set; }

        [Required(ErrorMessage = "File extension is required.")]
        public string FileExtension { get; set; }

        public TeamLogo(byte[] bytes, string fileExtension, Guid teamId)
        {
            Bytes = bytes;
            FileExtension = fileExtension;
            TeamId = teamId;
        }
    }
}
