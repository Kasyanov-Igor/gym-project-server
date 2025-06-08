using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services.Interface;

namespace gym_project_business_logic.Services
{
	public class AdminService : IAdminService
	{
		private ADatabaseConnection _connection;
		public AdminService(ADatabaseConnection connection)
		{
			this._connection = connection;
		}

		public async Task<bool> UpdateAdminAsync(int id, AdminDto adminDto)
		{
			var admin = await this._connection.Admins.FindAsync(id);
			if (admin == null) return false;

			admin.FullName = adminDto.FullName;
			admin.DateOfBirth = adminDto.DateOfBirth;
			admin.Email = adminDto.Email;
			admin.PhoneNumber = adminDto.PhoneNumber;
			admin.Gender = adminDto.Gender;
			admin.Status = adminDto.Status;
			admin.Login = adminDto.Login;

			if (!string.IsNullOrEmpty(adminDto.Password))
			{
				string salt = PasswordHelper.GenerateSalt();
				string hashedPassword = PasswordHelper.HashPassword(adminDto.Password, salt);

				admin.Salt = salt;
				admin.Password = hashedPassword;
			}

			await this._connection.SaveChangesAsync();
			return true;
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
	}
}
