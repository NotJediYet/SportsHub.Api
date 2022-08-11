using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsHub.Shared.Entities
{
    [Index(nameof(Headline), IsUnique = true)]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Picture { get; set; } = string.Empty;

        [Required(ErrorMessage = "Team id is required.")]
        public Guid TeamId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string AltPicture { get; set; } = string.Empty;

        [Required(ErrorMessage = "Headline article is required.")]
        public string Headline { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
        public bool IsShowComments { get; set; } = false;

        public Article(string picture, Guid teamId, string location, string altPicture, string headline, string caption, string context)
        {
            Picture = picture;
            TeamId = teamId;
            Location = location;
            AltPicture = altPicture;
            Headline = headline;
            Caption = caption;
            Context = context;
        }
    }
}

