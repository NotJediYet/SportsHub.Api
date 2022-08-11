using Microsoft.AspNetCore.Http;

namespace SportsHub.Shared.Models
{
    public class CreateLogoModel
    {
        public byte[] Bytes { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileExtension { get; set; }
        public Guid TeamId { get; set; }
    }
}
