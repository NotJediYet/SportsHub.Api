using SportsHub.Web.Models;
using SportsHub.Web.Repositories;

namespace SportsHub.Web.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ISubcategoryRepository Subcategories { get; }
        ITeamRepository Teams { get; }
        int Save();
    }
}
