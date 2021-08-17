using Kafka.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Kafka.Messages.UserRegistration;
using Kafka.Constants;
using System;
using System.Net;

namespace Kafka.Core.kafkaEvents.UserRegistration.Consumers
{
    public class RegisterUserConsumer : BackgroundService
    {
        private readonly IKafkaConsumer<string, RegisterUser> _consumer;

        public RegisterUserConsumer(IKafkaConsumer<string, RegisterUser> kafkaConsumer)
        {
            _consumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _consumer.Consume(KafkaTopics.RegisterUser, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{(int)HttpStatusCode.InternalServerError} Consume Failed On Topic - {KafkaTopics.RegisterUser}, {ex}");
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();

            base.Dispose();
        }
    }
}
