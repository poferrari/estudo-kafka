using Microsoft.Extensions.Logging;
using System.Threading;
using TemplateKafka.Consumer.Domain.Products.Dtos;
using TemplateKafka.Consumer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Consumer.Domain.Products.Services
{
    public class ConsumerProductService : IConsumerProductService
    {
        private readonly ILogger<ConsumerProductService> _logger;
        private readonly ITopicConsumer _topicConsumer;

        public ConsumerProductService(ILogger<ConsumerProductService> logger, ITopicConsumer topicConsumer)
        {
            _logger = logger;
            _topicConsumer = topicConsumer;
        }

        public void Subscribe(CancellationTokenSource cancellationTokenSource)
        {
            _topicConsumer.Consumer<ProductDto>("Post.Product", (message) => HandleMessage(message), cancellationTokenSource);
        }

        private void HandleMessage(ProductDto message)
        {

            _logger.LogInformation($"Get message: {message}");
        }
    }
}
