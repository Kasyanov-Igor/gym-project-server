using System;
using System.Collections.Generic;
using System.Text;

namespace gym_project_business_logic.Model.Domains
{
	public class DTOClient
	{
		public string Name { get; set; } = null!;

		public DateTime DateOfBirth { get; set; }

		public string ContactPhoneNumber { get; set; } = null!;

		public string EmailAddress { get; set; } = null!;

		public string Gender { get; set; } = null!;

		public string Login { get; set; } = null!;

		public string Password { get; set; } = null!;
	}
}
