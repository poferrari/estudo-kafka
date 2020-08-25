using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TemplateKafka.Consumer.Domain.Products.Dtos;
using TemplateKafka.Consumer.Domain.Products.Repositories;
using TemplateKafka.Consumer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Consumer.Domain.Products.Services
{
    public class ConsumerProductService : IConsumerProductService
    {
        private readonly ILogger<ConsumerProductService> _logger;
        private readonly ITopicConsumer _topicConsumer;
        private readonly IProductRepository _productRepository;

        public ConsumerProductService(ILogger<ConsumerProductService> logger,
                                      ITopicConsumer topicConsumer,
                                      IProductRepository productRepository)
        {
            _logger = logger;
            _topicConsumer = topicConsumer;
            _productRepository = productRepository;
        }

        public void Subscribe(CancellationTokenSource cancellationTokenSource)
        {
            _topicConsumer.Consumer<ProductDto>("Post.Product", async (message) => await HandleMessage(message), cancellationTokenSource);
        }

        private async Task HandleMessage(ProductDto product)
        {
            _logger.LogInformation($"Raw message received. Product: {product.Name}.");

            await InsertOrUpdateProduct(product);

            _logger.LogInformation($"Insert product: {product.Name} in Mongo DB");
        }

        private async Task InsertOrUpdateProduct(ProductDto product)
        {
            try
            {
                var productDb = await _productRepository.GetProduct(product.Id);

                if (productDb is null)
                {
                    await _productRepository.InsertProduct(product);
                }
                else
                {
                    await _productRepository.UpdateProduct(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
        }
    }
}
