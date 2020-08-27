using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private readonly ClientConfig _clientConfig;
        private readonly ProducerConfig _producerConfig;

        public TopicBroker(ILogger<TopicBroker> logger,
                           IMessageBuilder messageBuilder,
                           IOptions<KafkaConfig> kafkaConfig)
        {
            _logger = logger;
            _messageBuilder = messageBuilder;
            _kafkaConfig = kafkaConfig.Value;
            _clientConfig = CreateClientConfig();
            _producerConfig = CreateProducerConfig();
        }

        public async Task Publish<T>(string topicName, Message<T> message)
        {
            ValidateMessage(message);

            var body = _messageBuilder.SerializeAndEncodeMessage(message);

            var cancellationToken = new CancellationTokenSource(_kafkaConfig.TimeoutMs).Token;
            using var producer = new ProducerBuilder<string, string>(_producerConfig).Build();

            try
            {
                await PublishToTopic(producer, cancellationToken, body, topicName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message: {ex.Message}");
                throw;
            }
            finally
            {
                producer.Flush(cancellationToken);
            }
        }

        private void ValidateMessage<T>(Message<T> message)
        {
            if (message is null || message.Data is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
        }

        private async Task PublishToTopic(IProducer<string, string> producer, CancellationToken cancellationToken, Message<string, string> body, string topic)
        {
            try
            {
                var deliveryResult = await producer.ProduceAsync(topic, body, cancellationToken);

                _logger.LogInformation($"Delivered '{body.Value}|{string.Join(",", body.Headers.Select(t => t.Key))}' to '{deliveryResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, $"Error sending message: {ex.Error.Reason}");
                throw;
            }
        }

        public async Task CreateTopic(string topicName)
        {
            using var adminClient = new AdminClientBuilder(_clientConfig).Build();
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification>
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        NumPartitions = 2,
                        ReplicationFactor = 2
                    }
                });
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                {
                    _logger.LogError(e, $"An error occured creating topic {topicName}: {e.Results[0].Error.Reason}");
                }
                else
                {
                    _logger.LogError(e, "Topic already exists");
                }
            }
        }

        private ClientConfig CreateClientConfig()
            => new ClientConfig
            {
                BootstrapServers = _kafkaConfig.Brokers,
                SaslMechanism = SaslMechanism.Plain,
            };

        private ProducerConfig CreateProducerConfig()
           => new ProducerConfig(_clientConfig)
           {
               EnableIdempotence = true,
               Acks = Acks.All,
               Partitioner = Partitioner.Random,
               MessageSendMaxRetries = 5,
           };
    }
}
