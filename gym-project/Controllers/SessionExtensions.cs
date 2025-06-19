using Microsoft.AspNetCore.Mvc;

namespace gym_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionExtensions : ControllerBase
    {
        // Записать значение в сессию
        [HttpPost("set")]
        public IActionResult SetSessionValue(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
            return Ok($"Session value set: {key} = {value}");
        }

        // Получить значение из сессии
        [HttpGet("get")]
        public IActionResult GetSessionValue(string key)
        {
            var value = HttpContext.Session.GetString(key);
            if (value == null)
                return NotFound($"No session value found for key: {key}");

            return Ok(new { key, value });
        }

        // Удалить значение из сессии
        [HttpDelete("remove")]
        public IActionResult RemoveSessionValue(string key)
        {
            HttpContext.Session.Remove(key);
            return Ok($"Session value removed for key: {key}");
        }

        // Очистить всю сессию
        [HttpPost("clear")]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return Ok("Session cleared");
        }
    }
}
