using gym_project_business_logic.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace gym_project.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SessionController : ControllerBase
	{
		private ITokenService _tokenService;

		SessionController(ITokenService tokenService)
		{
			this._tokenService = tokenService;
		}

		[HttpGet("set")]
		public IActionResult SetSession(string token)
		{
			HttpContext.Session.SetString("token", token);
			return Ok("Session value set");
		}

		[HttpGet("get")]
		public IActionResult GetSession()
		{
			var userName = HttpContext.Session.GetString("UserName");
			if (string.IsNullOrEmpty(userName))
				return NotFound("Session value not found");

			return Ok($"UserName from session: {userName}");
		}
	}
}
