using System;
using System.Threading;

namespace TemplateKafka.Consumer.Infra.MessagingBroker.Interfaces
{
    public interface ITopicConsumer
    {
        void Consumer<T>(string topicName, Action<T> callback, CancellationTokenSource cancellationTokenSource);
    }
}
