namespace SportsHub.Shared.Models
{
    public class CreateArticleModel
    {
        public string Picture { get; set; } = string.Empty;
        public Guid TeamId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string AltPicture { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;

    }
}

