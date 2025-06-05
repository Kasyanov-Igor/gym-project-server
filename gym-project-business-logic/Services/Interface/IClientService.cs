using System;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services.Interface
{
	public interface IClientService
	{
		public bool FindClient(Client coach);

		public Task AddClient(Client user);

        public Task<bool> DeleteClientAsync(int id);

        public Task<Client?> GetClient(string login, string password);

		public Task<bool> GetEmail(string emailAddress);

        public Task<Client?> GetClientId(int id);

        public Task DeleteClientAsync(string login, string password);

		public Task UpdateClient(string login, string password, string? name = null, DateTime? dateOfBirth = null, string? contactPhoneNumber = null,
						string? emailAddress = null, string? gender = null, string? status = null, string? salt = null);

		public Task<bool> UpdateClientAsync(int clientId, DTOClient? newClient);

    }
}
