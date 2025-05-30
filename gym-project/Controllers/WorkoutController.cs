using AutoMapper;
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
    public class WorkoutController : ControllerBase
	{
        private MapperConfig _config;
        private IWorkoutService _service;
		private ILogger<GymsController> _logger;

		public WorkoutController(IWorkoutService service, ILogger<GymsController> logger, MapperConfig mapper)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._config = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<IEnumerable<Workout>> GetWorkouts()
        {
            return await this._service.GetWorkouts();
        }

        [HttpGet("ByCoach/{id}")]
        public async Task<IEnumerable<Workout>> GetWorkoutsByCoach(int id)
        {
            return await this._service.GetWorkoutsByCoach(id);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWorkout([FromForm] DTOWorkout createWorkout)
        {
            if (!ModelState.IsValid)
            {
                this._logger.LogWarning("Неверные данные при создании зала.");
                return BadRequest(ModelState);
            }

            Workout workout = this._config.CreateMapper().Map<Workout>(createWorkout);

            try
            {
                await this._service.AddWorkout(workout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при создании.");
            }

            this._logger.LogInformation($"(ID: {workout.Id}) успешно создан.");

            return Ok(new { Message = $"Тренировка '{workout.Title}' успешно создана." });
        }
    }
}
