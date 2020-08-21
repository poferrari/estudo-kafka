using Confluent.Kafka;
using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Interfaces
{
    public interface IMessageBuilder
    {
        Message<string, string> SerializeAndEncodeMessage<T>(Message<T> message);
    }
}
