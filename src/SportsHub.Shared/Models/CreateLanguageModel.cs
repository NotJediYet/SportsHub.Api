namespace SportsHub.Shared.Models
{
    public class CreateLanguageModel
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public bool? IsDefault { get; set; } = false;

        public bool? IsHidden { get; set; } = true;

        public bool? IsAdded { get; set; } = false;
    }
}
