using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace gym_project.Controllers
{
	public class WorkoutController : ControllerBase
	{
		private IWorkoutService _service;
		private ILogger<GymsController> _logger;

		public WorkoutController(IWorkoutService service, ILogger<GymsController> logger)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._service = service ?? throw new ArgumentNullException(nameof(service));
		}

        [HttpGet]
        public async Task<IEnumerable<Workout>> GetWorkouts()
        {
            return await this._service.GetWorkouts();
        }

        [HttpPost]
        public async Task<ActionResult> CreateGym([FromForm] Workout createWorkout)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogWarning("Неверные данные при создании зала.");
                return BadRequest(ModelState);
            }

            try
            {
                await this._service.AddWorkout(createWorkout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при создании.");
            }

            this._logger.LogInformation($"(ID: {createWorkout.Id}) успешно создан.");

            return Ok(new { Message = $"Тренировка '{createWorkout.Title}' успешно создана." });
        }
    }
}
