using gym_project_business_logic.Model;
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

		public CoachController()
		{
			this._connection = new SqliteConnection();
			this._coachService = new CoachService(this._connection);
		}

		[HttpPost("Register")]
		public void RegisterController([FromBody] Coach model)
		{
			if (ModelState.IsValid)
			{
				Coach coach = new Coach
				{
					FullName = model.FullName,
					DateOfBirth = model.DateOfBirth,
					Email = model.Email,
					PhoneNumber = model.PhoneNumber,
					Gender = model.Gender,
					Specialization = model.Specialization,
					Status = model.Status,
					Login = model.Login,
					Password = model.Password,
					WorkingTime = model.WorkingTime
				};

				if (coach.Password != null && coach.Password != null && coach.FullName != null)
				{
					this._coachService.Registration(coach);
				}
			}
		}
	}
}
