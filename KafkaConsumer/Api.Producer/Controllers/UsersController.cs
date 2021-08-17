using Kafka.Constants;
using Kafka.Interfaces;
using Kafka.Messages.UserRegistration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Api.Producer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class UsersController : ControllerBase
    {
        private readonly IKafkaProducer<string, RegisterUser> _kafkaProducer;

        public UsersController(IKafkaProducer<string, RegisterUser> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok("Users...");
        }

        [HttpPost]
        public async Task<IActionResult> ProduceMessage([BindRequired]RegisterUser request)
        {
            await _kafkaProducer.ProduceAsync(KafkaTopics.RegisterUser, null, request);

            return Ok("User Registration In Progress");
        }
    }
}
