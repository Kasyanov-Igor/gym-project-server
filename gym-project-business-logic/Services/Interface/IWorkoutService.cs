using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model.Domains;
using Model.Entities;

namespace gym_project_business_logic.Services.Interface
{
    public interface IWorkoutService
	{
		public Task AddWorkout(Workout workout);

        public Task<bool> DeleteWorkoutAsync(int id);

		public Task<Workout?> GetWorkout(string name);

		public Task<IEnumerable<Workout>> GetWorkouts();

		public Task<IEnumerable<Workout>> GetWorkoutsByCoach(int id);

        public Task<IEnumerable<Workout>> GetWorkoutsByGym(int id);

        public bool FindWorkout(Workout workout);

        public Task<bool> UpdateClientNameAsync(int workoutId, DTOWorkout? newWorkout);

        public Task<bool> AddClient(int workoutId, string? newClient);
    }
}
