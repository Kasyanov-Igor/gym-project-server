using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace gym_project_business_logic.Services.Interface
{
	public interface ITokenService
	{
		public string GenerateToken(IEnumerable<Claim> claims);
	}
}
