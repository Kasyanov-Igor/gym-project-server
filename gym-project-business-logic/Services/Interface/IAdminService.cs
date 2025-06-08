using System;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services.Interface
{
    public interface IAdminService
    {
        public Task<bool> UpdateAdminAsync(int id, AdminDto adminDto);

        public Task<Admin?> GetAdmin(string login, string password);

        public Task<bool> GetEmail(string emailAddress);
    }
}
