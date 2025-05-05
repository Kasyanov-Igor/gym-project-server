using System.Linq;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace gym_project_business_logic.Services
{
	public class ClientService : IClientService
	{
		private ADatabaseConnection _connection;

		public ClientService(ADatabaseConnection connection)
		{
			this._connection = connection;
		}

		public async Task AddClient(Client client)
		{
			if (!this.FindClient(client))
			{
				await this._connection.Clients.AddAsync(client);
				await this._connection.SaveChangesAsync();
			}
		}

		public async Task<bool> GetEmail(string emailAddress)
		{
			return !await this._connection.Clients.AnyAsync(u => u.EmailAddress == emailAddress);
		}

		public async Task<Client?> GetClient(string login, string password)
		{
			var user = await _connection.Clients.FirstOrDefaultAsync(u => u.Login == login);
			if (user == null) return null;

			string hashedPassword = PasswordHelper.HashPassword(password, user.Salt);

			return await _connection.Clients.FirstOrDefaultAsync(u => u.Login == login && u.Password == hashedPassword);
		}

		public bool FindClient(Client client)
		{
			if (this._connection.Clients.Any(usr =>
			usr.Login == client.Login))
			{
				return true;
			}

			return false;
		}
	}
}
