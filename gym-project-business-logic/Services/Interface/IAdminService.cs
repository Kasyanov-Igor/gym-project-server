using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Model.Enum;

namespace gym_project_business_logic.Services.Interface
{
	public interface IAdminService
	{
		public Task<IEnumerable<Admin>> GetAdminByIdAsync(int id);
		public Task<bool> UpdateAdminAsync(int id, AdminDto adminDto);
		public Task<bool> DeleteAdminAsync(int id);
		public Task<IEnumerable<Admin>> GetAllAdminsAsync();
		public bool FindAdmin(Admin admin);

		public Task AddAdmin(Admin admin);

		public Task<Admin?> GetAdmin(string login, string password);

		public Task<bool> GetEmail(string emailAddress);

		public Task DeleteAdminAsync(string login, string password);

		public Task UpdateAdmin(string login, string password, string? name = null, DateTime? dateOfBirth = null, string? contactPhoneNumber = null,
					string? emailAddress = null, string? gender = null, AdminStatus? status = null, string? salt = null);
	}
}
