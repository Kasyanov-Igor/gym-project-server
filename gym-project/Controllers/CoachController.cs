using AutoMapper;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CoachController : ControllerBase
	{
		private ICoachService _coachService;

		private MapperConfig _config;

		public CoachController(ICoachService coachService, MapperConfig mapper)
		{
			this._coachService = coachService;
			this._config = mapper;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterController([FromBody] DTOCoach modelDTO)
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

			if (!await this._coachService.GetEmail(modelDTO.Email))
			{
				ModelState.AddModelError("Email Address", "Этот адрес электронной почты уже зарегистрирован.");
				return BadRequest(ModelState);
			}

			try
			{
				await this._coachService.AddCoach(coach);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"Ошибка при сохранении пользователя: {ex}");
				return StatusCode(500, "Произошла ошибка при регистрации пользователя.");
			}


			return Ok(new { Message = "Пользователь успешно зарегистрирован." });

		}
	}

}

