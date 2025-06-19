using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace gym_project
{
	public class Startup
	{
        private readonly IConfiguration _configuration;
        private string _secretKey;
        private string _issuer;
        private string _audience;
        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT Secret Key is missing in configuration.");
            this._issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer", "JWT Issuer is missing in configuration.");
            this._audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience", "JWT Audience is missing in configuration.");
        }
        public void ConfigureServices(IServiceCollection services)
		{
            services.AddAuthentication("SomeOtherScheme")
            .AddJwtBearer("SomeOtherScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = this._issuer,
                    ValidateAudience = true,
                    ValidAudience = this._audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._secretKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("authToken"))
                        {
                            context.Token = context.Request.Cookies["authToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();

			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            app.UseRouting();

            app.UseAuthentication();

			app.UseAuthorization();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
