using System;
using TemplateKafka.Producer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Brokers
{
    public class Message<TData> : IMessage
    {
        public Guid CorrelationId { get; set; }
        public TData Data { get; set; }

        public static Message<TData> NewForData(TData data)
            => new Message<TData>
            {
                CorrelationId = Guid.NewGuid(),
                Data = data,
            };
    }
}
