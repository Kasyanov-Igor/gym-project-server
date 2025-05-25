using System.Threading.Tasks;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services.Interface
{
	public interface IWorkoutService
	{
		public Task AddWorkout(Workout workout);

		public Task DeleteWorkout(Workout workout);

		public Task<Workout?> GetWorkout(string name);
	}
}
