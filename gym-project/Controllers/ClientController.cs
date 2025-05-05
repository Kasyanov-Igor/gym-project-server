using AutoMapper;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using gym_project_business_logic.Services;
using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ClientController : ControllerBase
	{
		private ILogger<ClientController> _logger;
		private MapperConfig _mapper;
		private IClientService _clientService;
		private IWebHostEnvironment _environment;

		public ClientController(ILogger<ClientController> logger, MapperConfig mapper,
			IWebHostEnvironment environment, IClientService clientService)
		{
			this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this._environment = environment ?? throw new ArgumentNullException(nameof(environment));
			this._clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] DTOClient userDto)
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

			var user = this._mapper.CreateMapper().Map<Client>(userDto);

			string salt = PasswordHelper.GenerateSalt();
			string hashedPassword = PasswordHelper.HashPassword(user.Password, salt);

			user.Password = hashedPassword;
			user.Salt = salt;

			await this._clientService.AddClient(user);

			return Ok(new { Message = "Пользователь успешно зарегистрирован." });
		}
	}
}
