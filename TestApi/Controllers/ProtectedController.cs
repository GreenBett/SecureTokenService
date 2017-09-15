using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProtectedController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var claims = User.Claims.Select(x => new { x.Type, x.Value });
            return new JsonResult(claims);
        }

    }
}
