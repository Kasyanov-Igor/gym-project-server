using System.Security.Claims;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CoachController : ControllerBase
	{
		private MapperConfig _config;
		private ICoachService _coachService;
		private ITokenService _tokenService;
		private ILogger<CoachController> _logger;
		private IWebHostEnvironment _environment;

		public CoachController(ICoachService coachService, MapperConfig mapper, ILogger<CoachController> logger, IWebHostEnvironment environment,
					ITokenService tokenService)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._config = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
			this._coachService = coachService ?? throw new ArgumentNullException(nameof(coachService));
		}

		[HttpPost("register")]
		public IActionResult RegisterController([FromForm] DTOCoach modelDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			Coach coach = this._config.CreateMapper().Map<Coach>(modelDTO);


			string salt = PasswordHelper.GenerateSalt();
			string hashedPassword = PasswordHelper.HashPassword(coach.Password, salt);

			coach.Password = hashedPassword;
			coach.Salt = salt;

			if (!this._coachService.GetEmail(modelDTO.Email).Result)
			{
				ModelState.AddModelError("Email Address", "Этот адрес электронной почты уже зарегистрирован.");
				return BadRequest(ModelState);
			}

			try
			{
				this._coachService.AddCoach(coach);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Ошибка при сохранении пользователя: {ex}");
				return StatusCode(500, "Произошла ошибка при регистрации пользователя.");
			}


			return Ok(new { Message = "Пользователь успешно зарегистрирован." });

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] DTOLogin modelDTO)
		{
			if (!ModelState.IsValid)
			{
				return ValidationProblem();
			}

			Coach? user = await this._coachService.GetCoach(modelDTO.Login, modelDTO.Password);

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

			//var userDto = this._config.CreateMapper().Map<Coach>(user);

			return Ok(user);
		}

        [HttpGet("{id}")]
        public async Task<ActionResult<Coach>> GetCoach(int id)
        {
            return await this._coachService.GetCoachId(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Coach>> GetCoaches()
        {
            return await this._coachService.GetCoaches();
        }
    }
}

