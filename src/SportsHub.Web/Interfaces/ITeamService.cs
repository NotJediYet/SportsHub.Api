using SportsHub.Web.Models;

namespace SportsHub.Web.Interfaces
{
    public interface ITeamService
    {
        IEnumerable<Team> GetTeams();
        Team GetTeamByID(int id);
        void CreateTeam(string newName, int subcategoryId);
    }
}
