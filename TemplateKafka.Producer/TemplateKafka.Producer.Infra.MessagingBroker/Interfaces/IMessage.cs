using System;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Interfaces
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }
}
