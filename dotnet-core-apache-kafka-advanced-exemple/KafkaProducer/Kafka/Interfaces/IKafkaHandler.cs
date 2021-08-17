using System.Threading.Tasks;

namespace Kafka.Interfaces
{
    public interface IKafkaHandler<TKey, TValue>
    {
        Task HandleAsync(TKey key, TValue value);
    }
}
