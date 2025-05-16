using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ClientController : ControllerBase
	{
		private MapperConfig _mapper;
		private ITokenService _tokenService;
		private IClientService _clientService;
		private IWebHostEnvironment _environment;
		private ILogger<ClientController> _logger;

		public ClientController(ILogger<ClientController> logger, MapperConfig mapper,
			IWebHostEnvironment environment, IClientService clientService, ITokenService tokenService)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
			this._clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromForm] DTOClient userDto)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			if (!await this._clientService.GetEmail(userDto.EmailAddress))
			{
				ModelState.AddModelError("EmailAddress", "Этот адрес электронной почты уже зарегистрирован.");
				return ValidationProblem();
			}

			Client user = this._mapper.CreateMapper().Map<Client>(userDto);

			string salt = PasswordHelper.GenerateSalt();
			string hashedPassword = PasswordHelper.HashPassword(user.Password, salt);

			user.Password = hashedPassword;
			user.Salt = salt;

			await this._clientService.AddClient(user);

			return Ok(new { Message = "Пользователь успешно зарегистрирован." });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] DTOLogin userLoginDto)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			Client? user = await this._clientService.GetClient(userLoginDto.Login, userLoginDto.Password);

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
			///Client userReadDto = this._mapper.CreateMapper().Map<Client>(user);
			return Ok(user);
		}
	}
}
