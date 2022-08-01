using SportsHub.Web.AppData;
using SportsHub.Web.Interfaces;

namespace SportsHub.Web.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public ICategoryRepository Categories
        {
            get;
            private set;
        }

        public ISubcategoryRepository Subcategories
        {
            get;
            private set;
        }

        public ITeamRepository Teams
        {
            get;
            private set;
        }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(this._context);
            Subcategories = new SubcategoryRepository(this._context);
            Teams = new TeamRepository(this._context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
