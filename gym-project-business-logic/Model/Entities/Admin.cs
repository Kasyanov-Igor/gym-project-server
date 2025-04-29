using gym_project_business_logic.Model.Enum;
using System;

namespace gym_project_business_logic.Model
{
    public class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public AdminStatus? status { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
