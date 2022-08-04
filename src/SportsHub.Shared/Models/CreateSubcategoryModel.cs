namespace SportsHub.Shared.Models
{
    public class CreateSubcategoryModel
    {
        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
    }
}
