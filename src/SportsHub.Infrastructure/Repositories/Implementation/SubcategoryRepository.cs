using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.Repositories
{
    internal class SubcategoryRepository : GenericRepository<Subcategory>, ISubcategoryRepository
    {
        public SubcategoryRepository(SportsHubDbContext context) : base(context) { }
    }
}
