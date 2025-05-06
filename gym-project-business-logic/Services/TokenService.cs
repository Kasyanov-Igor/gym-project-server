using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gym_project_business_logic.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace gym_project_business_logic.Services
{
	public class TokenService : ITokenService
	{
		private string _secretKey;
		private string _issuer;
		private string _audience;
		private int _tokenLifetimeMinutes;

		public TokenService(IConfiguration configuration)
		{
			this._secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT Secret Key is missing in configuration.");
			this._issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer is missing in configuration.");
			this._audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT Audience is missing in configuration.");
			this._tokenLifetimeMinutes = configuration.GetValue<int>("Jwt:TokenLifetimeMinutes");
		}

		public string GenerateToken(IEnumerable<Claim> claims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._secretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: this._issuer,
				audience: this._audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(this._tokenLifetimeMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
