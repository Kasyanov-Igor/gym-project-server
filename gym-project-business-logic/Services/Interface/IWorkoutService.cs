using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model.Domains;
using Model.Entities;

namespace gym_project_business_logic.Services.Interface
{
	public interface IWorkoutService
	{
		public Task<IEnumerable<Workout>> GetWorkoutsByCoach(int id);
		public Task<IEnumerable<Workout>> GetWorkoutsByGym(int id);
		public Task<bool> UpdateClientNameAsync(int workoutId, DTOWorkout? newWorkout);
		public Task<bool> AddClient(int workoutId, string? newClient);
	}
}
