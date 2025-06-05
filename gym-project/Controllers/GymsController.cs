using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GymsController : ControllerBase
	{
		private IGymService _service;
		private ILogger<GymsController> _logger;

		public GymsController(IGymService service, ILogger<GymsController> logger)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
		public async Task<IEnumerable<Gym>> GetGyms()
		{
			return await this._service.GetGyms();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Gym>> GetGym(int id)
		{
			var gym = await this._service.GetGymByIdAsync(id);
			if (gym == null)
			{
				return NotFound();
			}
			return Ok(gym);
		}

		[HttpPost]
		public async Task<ActionResult> CreateGym([FromForm] Gym createGym)
		{
			if (!ModelState.IsValid)
			{
				this._logger.LogWarning("Неверные данные при создании зала.");
				return BadRequest(ModelState);
			}

			try
			{
				await this._service.AddGym(createGym);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Произошла ошибка при создании.");
			}

			this._logger.LogInformation($"Зал '{createGym.Name}' (ID: {createGym.Id}) успешно создан.");

			return Ok(new { Message = $"Зал '{createGym.Name}' успешно создан." });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAdmin(int id)
		{
			var deleted = await this._service.DeleteGymAsync(id);
			if (!deleted)
			{
				return NotFound();
			}

			return Ok(new { Message = $"Удаление прошло успешно" });
		}
	}
}
