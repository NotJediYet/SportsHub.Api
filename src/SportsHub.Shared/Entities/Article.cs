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
        public string Headline { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
        public bool IsShowComments { get; set; } = false;
        public Image Image { get; set; } = null;

        public Article(Guid teamId, string location, string headline, string caption, string context)
        {
            TeamId = teamId;
            Location = location;
            Headline = headline;
            Caption = caption;
            Context = context;
        }
    }
}