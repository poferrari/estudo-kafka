using Confluent.Kafka;
using JsonApiSerializer;
using Newtonsoft.Json;
using System;
using TemplateKafka.Producer.Infra.MessagingBroker.Configs;
using TemplateKafka.Producer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Brokers
{
    public class MessageBuilder : IMessageBuilder
    {
        public Message<string, string> SerializeAndEncodeMessage<T>(Message<T> message)
        {
            var serialized = JsonConvert.SerializeObject(message.Data, new JsonApiSerializerSettings());
            return new Message<string, string>
            {
                Value = serialized,
                Headers = new Headers
                {
                    { KafkaParameter.CorrelationIdHeader, message.CorrelationId.ToByteArray() }
                },
                Timestamp = new Timestamp(DateTime.Now, TimestampType.CreateTime)
            };
        }
    }
}
