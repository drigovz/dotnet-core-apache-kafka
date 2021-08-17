using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace ConsumerWebApp.Controllers
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

        [HttpGet]
        public IActionResult Index()
        {
            string resultComsumer = "";

            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServer,
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_topic);
                var cancelToken = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                        var message = consumer.Consume(cancelToken.Token);

                        _logger.LogInformation($"Message: {message.Message.Value} received from {message.TopicPartitionOffset}");
                        resultComsumer += $"Message: {message.Message.Value} received from {message.TopicPartitionOffset}";
                        return StatusCode(StatusCodes.Status200OK, resultComsumer);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }

                consumer.Close();
            }

            return Ok();
        }
    }
}
