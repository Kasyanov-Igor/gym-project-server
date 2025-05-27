using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace gym_project_business_logic.Services
{
    public class WorkoutService : IWorkoutService
	{
		private ADatabaseConnection _connection;
		public WorkoutService(ADatabaseConnection databaseConnection)
		{
			this._connection = databaseConnection;
		}

		public async Task AddWorkout(Workout workout)
		{
			await this._connection.Workouts.AddAsync(workout);
			await this._connection.SaveChangesAsync();
		}

		public async Task DeleteWorkout(Workout workout)
		{
			this._connection.Workouts.Remove(workout);
			this._connection.SaveChanges();
		}

		public async Task<Workout?> GetWorkout(string name)
		{
			return await this._connection.Workouts.FirstOrDefaultAsync(u => u.Title == name);
		}

        public async Task<IEnumerable<Workout>> GetWorkouts()
        {
            return await this._connection.Workouts.ToListAsync();
        }
    }
}
