using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services.Interface;

namespace gym_project_business_logic.Services
{
	public class ClientService : IClientService
	{
		private ADatabaseConnection _connection;

		public ClientService(ADatabaseConnection connection)
		{
			this._connection = connection;
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

		public async Task<bool> UpdateClientAsync(int clientId, DTOClient? newClient)
		{
			var client = await _connection.Clients.FindAsync(clientId);
			if (client == null)
			{
				return false; // клиент не найден
			}

			if (newClient != null)
			{
				client.Name = newClient.Name;
				client.DateOfBirth = newClient.DateOfBirth;
				client.ContactPhoneNumber = newClient.ContactPhoneNumber;
				client.EmailAddress = newClient.EmailAddress;
				client.Gender = newClient.Gender;
				client.Login = newClient.Login;

				if (!string.IsNullOrEmpty(newClient.Password))
				{
					string salt = PasswordHelper.GenerateSalt();
					string hashedPassword = PasswordHelper.HashPassword(newClient.Password, salt);

					client.Salt = salt;
					client.Password = hashedPassword;
				}

				try
				{
					await _connection.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!this._connection.Clients.Any(c => c.Id == clientId))
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
