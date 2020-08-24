using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TemplateKafka.Producer.Infra.MessagingBroker.Configs;
using TemplateKafka.Producer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Brokers
{
    public class TopicBroker : ITopicBroker
    {
        private readonly ILogger<TopicBroker> _logger;
        private readonly IMessageBuilder _messageBuilder;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ProducerConfig _producerConfig;

        public TopicBroker(ILogger<TopicBroker> logger,
                           IMessageBuilder messageBuilder,
                           KafkaConfig kafkaConfig)
        {
            _logger = logger;
            _messageBuilder = messageBuilder;
            _kafkaConfig = kafkaConfig;
            _producerConfig = CreateProducerConfig();
        }

        public async Task Publish<T>(string topicName, Message<T> message)
        {
            ValidateMessage(message);

            var body = _messageBuilder.SerializeAndEncodeMessage(message);

            try
            {
                using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();

                await PublishToTopic(producer, body, topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message: {ex.Message}");
                throw;
            }
        }

        private void ValidateMessage<T>(Message<T> message)
        {
            if (message is null || message.Data is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
        }

        private async Task PublishToTopic(IProducer<string, string> producer, Message<string, string> body, string topic)
        {
            try
            {
                var cancellationToken = new CancellationTokenSource(_kafkaConfig.TimeoutMs);

                var deliveryResult = await producer.ProduceAsync(topic, body, cancellationToken.Token);

                _logger.LogInformation($"Delivered '{body.Value}|{string.Join(",", body.Headers.Select(t => t.Key))}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, $"Error sending message: {ex.Error.Reason}");
                throw;
            }
        }

        private ProducerConfig CreateProducerConfig()
           => new ProducerConfig
           {
               BootstrapServers = _kafkaConfig.Brokers,
               SaslMechanism = SaslMechanism.Plain,
               EnableIdempotence = true,
               Acks = Acks.All,
               MessageSendMaxRetries = 3,
           };
    }
}
