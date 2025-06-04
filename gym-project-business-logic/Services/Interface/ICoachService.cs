using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using Model.Entities;

namespace gym_project_business_logic.Services.Interface
{
	public interface ICoachService
	{
		public Task<Coach?> GetCoach(string login, string password);

		public void AddCoach(Coach user);

		public Task<bool> GetEmail(string emailAddress);

		public bool FindCoach(Coach user);

		public bool DeleteCoach(string LoginCoach, string password);

		public Task<Coach?> GetCoachId(int id);

        public Task<IEnumerable<Coach>> GetCoaches();

    }
}
