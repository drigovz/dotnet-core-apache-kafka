using System.Threading.Tasks;

namespace Kafka.Interfaces
{
    public interface IKafkaProducer<in TKey, in TValue> where TValue : class
    {
        Task ProduceAsync(string topic, TKey key, TValue value);
    }
}
