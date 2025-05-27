using gym_project_business_logic.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gym_project_business_logic.Services.Interface
{
    public interface IGymService
    {
        public Task AddGym(Gym gym);

        public Task<IEnumerable<Gym>> GetGyms();
    }
}
