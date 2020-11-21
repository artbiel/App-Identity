using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Identity.Api.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
