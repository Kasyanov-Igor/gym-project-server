using gym_project_business_logic.Model;
using System.Threading.Tasks;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace gym_project_business_logic.Services
{
	public class GymService : IGymService
	{
		private ADatabaseConnection _connection;

		public GymService(ADatabaseConnection databaseConnection) => this._connection = databaseConnection;
		public async Task AddGym(Gym gym)
		{
			await this._connection.Gyms.AddAsync(gym);
			await this._connection.SaveChangesAsync();
		}

		public async Task<IEnumerable<Gym>> GetGyms()
		{
			return await this._connection.Gyms.ToListAsync();
		}

		public async Task<bool> DeleteGymAsync(int id)
		{
			var gym = await this._connection.Gyms.FindAsync(id);
			if (gym == null) return false;

			this._connection.Gyms.Remove(gym);
			await this._connection.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<Gym>> GetGymByIdAsync(int id)
		{
			var allGyms = await this._connection.Gyms.ToListAsync();

			return allGyms.Where(u => u.Id == id);
		}
	}
}
