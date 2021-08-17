using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProducerWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class HomeController : Controller
    {
        private readonly string _bootstrapServer = "localhost:9092";
        private readonly string _topic = "queue_order";
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index()
        {
            string message = "Hello from producer";
            var config = new ProducerConfig { BootstrapServers = _bootstrapServer };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var sendResult = producer.ProduceAsync(_topic, new Message<Null, string> { Value = message })
                                             .GetAwaiter()
                                             .GetResult();

                    _logger.LogInformation($"Message: '{sendResult.Message.Value}' of '{sendResult.TopicPartitionOffset}'");
                    return StatusCode(StatusCodes.Status200OK, $"Message: '{sendResult.Message.Value}' of '{sendResult.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    _logger.LogInformation($"Delivery failed: {e.Error.Reason}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
