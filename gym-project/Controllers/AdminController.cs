using gym_project_business_logic.Model;
using System.Security.Claims;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace gym_project.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private MapperConfig _mapper;
		private ITokenService _tokenService;
		private IAdminService _adminService;
		private IWebHostEnvironment _environment;
		private ILogger<ClientController> _logger;

		public AdminController(IAdminService adminService, ILogger<ClientController> logger, MapperConfig mapper,
			IWebHostEnvironment environment, ITokenService tokenService)
		{
			this._adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmins()
		{
			var admins = await this._adminService.GetAllAdminsAsync();
			return Ok(admins);
		}

		// GET: api/admin/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<AdminDto>> GetAdmin(int id)
		{
			var admin = await this._adminService.GetAdminByIdAsync(id);
			if (admin == null)
			{
				return NotFound();
			}
			return Ok(admin);
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromForm] AdminDto userDto)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			if (!await this._adminService.GetEmail(userDto.Email))
			{
				ModelState.AddModelError("EmailAddress", "Этот адрес электронной почты уже зарегистрирован.");
				return ValidationProblem();
			}

			Admin user = this._mapper.CreateMapper().Map<Admin>(userDto);

			string salt = PasswordHelper.GenerateSalt();
			string hashedPassword = PasswordHelper.HashPassword(user.Password, salt);

			user.Password = hashedPassword;
			user.Salt = salt;

			await this._adminService.AddAdmin(user);

			return Ok(new { Message = "Пользователь успешно зарегистрирован." });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] DTOLogin userLoginDto)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			Admin? user = await this._adminService.GetAdmin(userLoginDto.Login, userLoginDto.Password);

			if (user == null)
			{
				this._logger.LogWarning("Неудачная попытка входа: неверный логин или пароль.");

				return Unauthorized(new ProblemDetails
				{
					Title = "Неверные учетные данные",
					Detail = "Логин или пароль указаны неверно",
					Status = StatusCodes.Status401Unauthorized
				});
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, user.Status.ToString())
			};

			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = _environment.IsProduction(),
				SameSite = SameSiteMode.Lax,
				Expires = DateTimeOffset.UtcNow.AddMinutes(30)
			};
			var token = this._tokenService.GenerateToken(claims);

			Response.Cookies.Append("authToken", token, cookieOptions);

			return Ok(user.Id);
		}

		// PUT: api/admin/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAdmin(int id, [FromBody] AdminDto adminDto)
		{
			if (adminDto == null)
			{
				return BadRequest("Invalid admin data.");
			}

			var updated = await _adminService.UpdateAdminAsync(id, adminDto);
			if (!updated)
			{
				return NotFound();
			}

			return Ok(new { Message = $"Обновление админа прошло успешно" });
		}

		// DELETE: api/admin/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			var deleted = await _adminService.DeleteAdminAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return Ok(new { Message = $"Удаление админа прошло успешно" });
		}
	}
}
