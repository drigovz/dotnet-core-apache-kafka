using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Interfaces
{
    public interface IKafkaConsumer<in TKey, in TValue> where TValue : class
    {
        Task Consume(string topic, CancellationToken stoppingToken);
        void Close();
        void Dispose();
    }
}
