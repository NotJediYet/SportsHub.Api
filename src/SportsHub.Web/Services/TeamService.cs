 using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Team> GetTeams()
        {
            return _unitOfWork.Teams.Get().OrderByDescending(t => t.Id);
        }

        public Team GetTeamByID(int id)
        {
            Team? team = _unitOfWork.Teams.GetByID(id);
            if (team == null)
            {
                throw new ApplicationException("Team with that id is not found");
            }
            return team;
        }

        public void CreateTeam(string newName, int subcategoryId)
        {
            if (newName == null)
            {
                throw new ApplicationException("Team name can't be null");
            }
            if (checkIfSubcategoryNotExists(subcategoryId))
            {
                throw new ApplicationException(
                    "Subcategory with that id doesn't exists!");
            }
            if (chechIfNameNotUnique(newName))
            {
                throw new ApplicationException(
                    "Team with that name already exists!");
            }
            _unitOfWork.Teams.Add(new Team(newName, subcategoryId));
            _unitOfWork.Save();
        }

        private bool chechIfNameNotUnique(string newName)
        {
            return _unitOfWork.Teams.Get()
                    .Any(item => item.Name == newName);
        }

        private bool checkIfSubcategoryNotExists(int subcategoryId)
        {
            return _unitOfWork.Subcategories.GetByID(subcategoryId) == null;
        }

    }
}
