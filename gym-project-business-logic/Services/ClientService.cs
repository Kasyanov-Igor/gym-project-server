using System;
using System.Linq;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
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

		public async Task UpdateClient(string login, string password, string? name = null, DateTime? dateOfBirth = null, string? contactPhoneNumber = null,
							string? emailAddress = null, string? gender = null, string? status = null, string? salt = null)
		{
			Client? client = await GetClient(login, password);

			if (client != null)
			{
				if (name != null)
				{
					client.Name = name;
				}
				if (dateOfBirth != null)
				{
					client.DateOfBirth = dateOfBirth.Value;
				}

				if (contactPhoneNumber != null)
				{
					client.ContactPhoneNumber = contactPhoneNumber;
				}
				if (emailAddress != null)
				{
					client.EmailAddress = emailAddress;
				}
				if (gender != null)
				{
					client.Gender = gender;
				}
				if (status != null)
				{
					client.Status = status;
				}
				if (salt != null)
				{
					client.Salt = salt;
				}
				await this.DeleteClientAsync(login, password);
				await this.AddClient(client);
			}
		}

		public async Task DeleteClientAsync(string login, string password)
		{
			Client? client = await GetClient(login, password);
			if (client != null)
			{
				this._connection.Clients.Remove(client);
				await this._connection.SaveChangesAsync();
			}
		}
		public async Task<Client?> GetClientId(int id)
		{
			return await this._connection.Clients.FirstOrDefaultAsync(u => u.Id == id);
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
		public async Task<bool> DeleteClientAsync(int id)
		{
			var clients = await this._connection.Clients.FindAsync(id);
			if (clients == null) return false;

			this._connection.Clients.Remove(clients);
			await this._connection.SaveChangesAsync();
			return true;
		}
	}
}
