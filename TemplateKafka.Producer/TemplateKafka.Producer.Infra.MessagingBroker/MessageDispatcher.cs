using System;
using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;
using TemplateKafka.Producer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Producer.Infra.MessagingBroker
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly ITopicBroker _topicBroker;

        public MessageDispatcher(ITopicBroker topicBroker)
        {
            _topicBroker = topicBroker;
        }

        public void PublishToTopic<T>(string topicName, Message<T> message)
        {
            if (topicName is null)
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _topicBroker.Publish(topicName, message);
        }
    }
}
