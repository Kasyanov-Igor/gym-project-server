using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gym_project_business_logic.Model.Domains;
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
			if (!this.FindWorkout(workout))
			{
				await this._connection.Workouts.AddAsync(workout);
				await this._connection.SaveChangesAsync();
			}
		}

		public bool FindWorkout(Workout workout)
		{
			if (this._connection.Workouts.Any(usr =>
			usr.BookingTime == workout.BookingTime))
			{
				return true;
			}

			return false;
		}

		public async Task<bool> DeleteWorkoutAsync(int id)
		{
			var work = await this._connection.Workouts.FindAsync(id);
			if (work == null) return false;

			this._connection.Workouts.Remove(work);
			await this._connection.SaveChangesAsync();
			return true;
		}

		public async Task<Workout?> GetWorkout(string name)
		{
			return await this._connection.Workouts.FirstOrDefaultAsync(u => u.Title == name);
		}

		public async Task<IEnumerable<Workout>> GetWorkouts()
		{
			return await this._connection.Workouts.ToListAsync();
		}

		public async Task<IEnumerable<Workout>> GetWorkoutsByCoach(int id)
		{
			var allWorkouts = await this._connection.Workouts.ToListAsync();

			return allWorkouts.Where(workout => workout.CoachId == id);
		}

		public async Task<IEnumerable<Workout>> GetWorkoutsByGym(int id)
		{
			var allWorkouts = await this._connection.Workouts.ToListAsync();

			return allWorkouts.Where(workout => workout.GymId == id);
		}

		public async Task<bool> UpdateClientNameAsync(int workoutId, DTOWorkout? newWorkout)
		{
			var workout = await _connection.Workouts.FindAsync(workoutId);
			if (workout == null)
			{
				return false; // не найден
			}

			if (newWorkout != null)
			{
				workout.Title = newWorkout.Title;
				workout.NameCoach = newWorkout.NameCoach;
				workout.GymId = newWorkout.GymId;
				workout.Places = newWorkout.Places;
				workout.ClientName = newWorkout.ClientName;
				workout.Description = newWorkout.Description;
				workout.DurationMinutes = newWorkout.DurationMinutes;
				workout.BookingTime = newWorkout.BookingTime;
				workout.CoachId = newWorkout.CoachId;

				try
				{
					await _connection.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!this._connection.Workouts.Any(w => w.Id == workoutId))
					{
						return false;
					}
					throw;
				}
			}

			return true;
		}

		public async Task<bool> AddClient(int workoutId, string? newClient)
		{
			var workout = await _connection.Workouts.FindAsync(workoutId);
			if (workout == null)
			{
				return false; // не найден
			}

			if (newClient != null)
			{
				workout.ClientName = newClient;
				//if (workout.Places > 0)
				//{
				//	workout.Places -= 1;
				//}
				//else
				//{
				//	return false;
				//}

				try
				{
					await _connection.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!this._connection.Workouts.Any(w => w.Id == workoutId))
					{
						return false;
					}
					throw;
				}
			}

			return true;
		}

	}
}
