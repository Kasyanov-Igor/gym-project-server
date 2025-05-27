using gym_project_business_logic.Model;
using System.Threading.Tasks;
using gym_project_business_logic.Services.Interface;
using gym_project_business_logic.Model.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace gym_project_business_logic.Services
{
    public class GymService: IGymService
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
    }
}
