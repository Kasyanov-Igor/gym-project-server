using System;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services.Interface
{
	public interface IClientService
	{
		public Task<Client?> GetClient(string login, string password);
		public Task<bool> GetEmail(string emailAddress);
		public Task<bool> UpdateClientAsync(int clientId, DTOClient? newClient);
	}
}
