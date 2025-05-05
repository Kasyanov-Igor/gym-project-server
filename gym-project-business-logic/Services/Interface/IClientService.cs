using System.Threading.Tasks;
using gym_project_business_logic.Model;

namespace gym_project_business_logic.Services.Interface
{
	public interface IClientService
	{
		public bool FindClient(Client coach);

		public Task AddClient(Client user);

		public Task<Client?> GetClient(string login, string password);

		public Task<bool> GetEmail(string emailAddress);
	}
}
