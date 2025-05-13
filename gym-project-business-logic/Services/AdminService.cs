using gym_project_business_logic.Model;
using System.Threading.Tasks;
using gym_project_business_logic.Services.Interface;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using gym_project_business_logic.Model.Enum;

namespace gym_project_business_logic.Services
{
	public class AdminService : IAdminService
	{
		private ADatabaseConnection _connection;
		public AdminService(ADatabaseConnection connection)
		{
			this._connection = connection;
		}
		public async Task AddAdmin(Admin admin)
		{
			if (!this.FindAdmin(admin))
			{
				await this._connection.Admins.AddAsync(admin);
				await this._connection.SaveChangesAsync();
			}
		}

		public bool FindAdmin(Admin admin)
		{
			if (this._connection.Admins.Any(usr =>
			usr.Login == admin.Login))
			{
				return true;
			}

			return false;
		}

		public async Task<bool> GetEmail(string emailAddress)
		{
			return !await this._connection.Admins.AnyAsync(u => u.Email == emailAddress);
		}

		public async Task<Admin?> GetAdmin(string login, string password)
		{
			var user = await _connection.Clients.FirstOrDefaultAsync(u => u.Login == login);
			if (user == null) return null;

			string hashedPassword = PasswordHelper.HashPassword(password, user.Salt);

			return await _connection.Admins.FirstOrDefaultAsync(u => u.Login == login && u.Password == hashedPassword);
		}

		public async Task UpdateAdmin(string login, string password, string? name = null, DateTime? dateOfBirth = null, string? contactPhoneNumber = null,
					string? emailAddress = null, string? gender = null, AdminStatus? status = null, string? salt = null)
		{
			Admin? admin = await GetAdmin(login, password);

			if (admin != null)
			{
				if (name != null)
				{
					admin.FullName = name;
				}
				if (dateOfBirth != null)
				{
					admin.DateOfBirth = dateOfBirth.Value;
				}

				if (contactPhoneNumber != null)
				{
					admin.PhoneNumber = contactPhoneNumber;
				}
				if (emailAddress != null)
				{
					admin.Email = emailAddress;
				}
				if (gender != null)
				{
					admin.Gender = gender;
				}
				if (status != null)
				{
					admin.Status = status;
				}
				if (salt != null)
				{
					admin.Salt = salt;
				}
				await this.DeleteAdminAsync(login, password);
				await this.AddAdmin(admin);
			}
		}

		public async Task DeleteAdminAsync(string login, string password)
		{
			Admin? admin = await GetAdmin(login, password);
			if (admin != null)
			{
				this._connection.Admins.Remove(admin);
				await this._connection.SaveChangesAsync();
			}
		}
	}
}
