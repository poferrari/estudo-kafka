using Confluent.Kafka;
using JsonApiSerializer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using TemplateKafka.Consumer.Infra.MessagingBroker.Configs;
using TemplateKafka.Consumer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Consumer.Infra.MessagingBroker.Brokers
{
    public class TopicConsumer : ITopicConsumer
    {
        private readonly ILogger<TopicConsumer> _logger;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ConsumerConfig _consumerConfig;
        private static JsonApiSerializerSettings _jsonApiSerializerSettings = new JsonApiSerializerSettings();

        public TopicConsumer(ILogger<TopicConsumer> logger,
                             KafkaConfig kafkaConfig)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig;
            _consumerConfig = CreateConsumerConfig();
        }

        public void Consumer<T>(string topicName, Action<T> callback, CancellationTokenSource cancellationTokenSource)
        {
            Validate(topicName);

            using var consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();
            consumer.Subscribe(topicName);

            try
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var consumerResult = consumer.Consume(cancellationTokenSource.Token);

                        T message = JsonConvert.DeserializeObject<T>(consumerResult.Message.Value, _jsonApiSerializerSettings);

                        callback(message);

                        consumer.Commit(consumerResult);

                        _logger.LogInformation($"Consumed message '{consumerResult.Message.Value}' at: '{consumerResult.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occured in consumer: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation canceled");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured: {ex.Message}");
            }
            finally
            {
                consumer.Close();
            }
        }

        private void Validate(string topicName)
        {
            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName));
            }
        }

        private ConsumerConfig CreateConsumerConfig()
          => new ConsumerConfig
          {
              BootstrapServers = _kafkaConfig.Brokers,
              ClientId = $"{Environment.MachineName}_{Guid.NewGuid()}",
              GroupId = _kafkaConfig.GroupId,
              SaslMechanism = SaslMechanism.Plain,
              AutoOffsetReset = AutoOffsetReset.Earliest,
              MaxPollIntervalMs = _kafkaConfig.MaxPollIntervalMs,
              EnableAutoCommit = false,
              EnableAutoOffsetStore = false,
              EnableSslCertificateVerification = false,
              Acks = Acks.All,
          };
    }
}
