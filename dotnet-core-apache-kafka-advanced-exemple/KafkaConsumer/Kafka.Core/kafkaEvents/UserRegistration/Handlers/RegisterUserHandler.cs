using Kafka.Constants;
using Kafka.Interfaces;
using Kafka.Messages.UserRegistration;
using System;
using System.Threading.Tasks;

namespace Kafka.Core.kafkaEvents.UserRegistration.Handlers
{
    public class RegisterUserHandler : IKafkaHandler<string, RegisterUser>
    {
        private readonly IKafkaProducer<string, UserRegistered> _producer;

        public RegisterUserHandler(IKafkaProducer<string, UserRegistered> producer)
        {
            _producer = producer;
        }

        public Task HandleAsync(string key, RegisterUser value)
        {
            Console.WriteLine($"Consuming UserRegistered topic message with the below data\n FirstName: {value.FirstName}\n LastName: {value.LastName}\n UserName: {value.UserName}\n EmailId: {value.EmailId}");

            _producer.ProduceAsync(KafkaTopics.UserRegistered, "", new UserRegistered { UserId = 1 });

            return Task.CompletedTask;
        }
    }
}
