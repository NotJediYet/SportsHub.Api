using SportsHub.Web.AppData;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Repositories
{
    public class SubcategoryRepository : GenericRepository<Subcategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(AppDbContext context) : base(context) { }
    }
}
