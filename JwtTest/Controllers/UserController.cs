using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet] 
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "Shalitha Viraj", "Shali"
            };
        }
    }
}
