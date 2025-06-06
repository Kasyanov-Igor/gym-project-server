using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using Model.Entities;

namespace gym_project_business_logic.Services.Interface
{
	public interface ICoachService
	{
		public Task<Coach?> GetCoach(string login, string password);

		public Task AddCoach(Coach user);

		public Task<bool> GetEmail(string emailAddress);

        public Task<bool> UpdateCoachAsync(int coachId, DTOCoach? newCoach);

        public bool FindCoach(Coach user);

        public Task<bool> DeleteCoachAsync(int id);

        public bool DeleteCoach(string LoginCoach, string password);

		public Task<Coach?> GetCoachId(int id);

        public Task<IEnumerable<Coach>> GetCoaches();

    }
}
