using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services.Interface
{
	public interface ICoachService
	{
		public Task<Coach?> GetCoach(string login, string password);
		public Task<bool> GetEmail(string emailAddress);
		public Task<bool> UpdateCoachAsync(int coachId, DTOCoach? newCoach);
	}
}
