using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;

namespace TemplateKafka.Producer.Infra.MessagingBroker
{
    public interface IMessageDispatcher
    {
        void PublishToTopic<T>(string topicName, Message<T> message);
    }
}
