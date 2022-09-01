using Microsoft.AspNetCore.Http;

namespace SportsHub.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] ToByteArray(this IFormFile fileLogo)
        {
            using var memoryStream = new MemoryStream();
            fileLogo.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}