using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class CreateTeamLogoModel
    {
        public byte[] Bytes { get; set; }

        public string FileExtension { get; set; }

        public decimal Size { get; set; }

        public Guid TeamId { get; set; }
    }
}
