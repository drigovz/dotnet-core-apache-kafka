using Confluent.Kafka;
using Kafka.Consumer;
using Kafka.Core.kafkaEvents.UserRegistration.Consumers;
using Kafka.Core.kafkaEvents.UserRegistration.Handlers;
using Kafka.Interfaces;
using Kafka.Messages.UserRegistration;
using Kafka.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var clientConfig = new ClientConfig()
            {
                BootstrapServers = Configuration.GetValue<string>("Kafka:ClientConfigs:BootstrapServers")
            };

            var producerConfig = new ProducerConfig(clientConfig);
            var consumerConfig = new ConsumerConfig(clientConfig)
            {
                GroupId = "SourceApp",
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000
            };

            services.AddSingleton(producerConfig);
            services.AddSingleton(consumerConfig);

            services.AddSingleton(typeof(IKafkaProducer<,>), typeof(KafkaProducer<,>));

            services.AddScoped<IKafkaHandler<string, RegisterUser>, RegisterUserHandler>();
            services.AddSingleton(typeof(IKafkaConsumer<,>), typeof(KafkaConsumer<,>));
            services.AddHostedService<RegisterUserConsumer>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
