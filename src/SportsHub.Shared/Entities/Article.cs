using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Required(ErrorMessage = "Team id is required.")]
        public Guid TeamId { get; set; }

        public string Location { get; set; } = string.Empty;

        public string AltImage { get; set; } = string.Empty;

        public string Headline { get; set; } = string.Empty;

        public string Caption { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public bool IsPublished { get; set; } = false;

        public bool IsShowComments { get; set; } = false;

        [FromForm]
        [NotMapped]
        public IFormFile Image { get; set;}

        public Article(Guid teamId, string location, string altImage, string headline, string caption, string content, bool isShowComments)
        {
            TeamId = teamId;
            Location = location;
            AltImage = altImage;
            Headline = headline;
            Caption = caption;
            Content = content;
            IsShowComments = isShowComments;
        }
    }
}