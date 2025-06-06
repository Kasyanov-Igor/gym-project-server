using System;
using System.Collections.Generic;
using System.Text;
using gym_project_business_logic.Model.Enum;

namespace gym_project_business_logic.Model.Domains
{
    public class AdminDto
    {
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public AdminStatus? Status { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string? Password { get; set; }
    }
}
