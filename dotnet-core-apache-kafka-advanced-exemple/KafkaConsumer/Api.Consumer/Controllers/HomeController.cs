using Microsoft.AspNetCore.Mvc;

namespace Api.Consumer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok("Welcome to Kafka Consumer Application....");
        }
    }
}
