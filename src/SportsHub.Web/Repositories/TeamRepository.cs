using SportsHub.Web.AppData;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Repositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context) { }
    }
}
