using SportsHub.Web.AppData;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

    }
}
