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

		private ADatabaseConnection _connection;

		private IMapper _mapper;

		public CoachController(IMapper mapper)
		{
			this._connection = new SqliteConnection();
			this._coachService = new CoachService(this._connection);
			this._mapper = mapper;

		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterController([FromBody] DTOCoach modelDTO)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Coach coach = this._mapper.Map<Coach>(modelDTO);

			string salt = PasswordHelper.GenerateSalt();
			string hashedPassword = PasswordHelper.HashPassword(coach.Password, salt);

            if (!await this._coachService.GetEmail(modelDTO.Email))
            {
                ModelState.AddModelError("Email Address", "���� ����� ����������� ����� ��� ���������������.");
                return BadRequest(ModelState);
            }

            try
            {
                await this._coachService.AddCoach(coach);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"������ ��� ���������� ������������: {ex}");
                return StatusCode(500, "��������� ������ ��� ����������� ������������.");
            }
            

            return Ok(new { Message = "������������ ������� ���������������." });

        }
	}

}

