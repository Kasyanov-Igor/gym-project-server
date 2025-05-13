using System;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Enum;

namespace gym_project_business_logic.Services.Interface
{
	public interface IAdminService
	{
		public bool FindAdmin(Admin admin);

		public Task AddAdmin(Admin admin);

		public Task<Admin?> GetAdmin(string login, string password);

		public Task<bool> GetEmail(string emailAddress);

		public Task DeleteAdminAsync(string login, string password);

		public Task UpdateAdmin(string login, string password, string? name = null, DateTime? dateOfBirth = null, string? contactPhoneNumber = null,
                    string? emailAddress = null, string? gender = null, AdminStatus? status = null, string? salt = null);
	}
}
