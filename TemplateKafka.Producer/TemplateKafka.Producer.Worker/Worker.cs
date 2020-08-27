using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Services;

namespace TemplateKafka.Producer.Worker
{
    public class Worker : BackgroundService
    {
        private const bool TopicCreated = true;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private static bool _isTopicCreated = false;

        public Worker(ILogger<Worker> logger,
                      IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

            if (!_isTopicCreated)
            {
                await productService.CreateTopic("Post.Product");
                _isTopicCreated = TopicCreated;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await productService.InsertProducts();

                    //await productService.UpdateProducts();

                    _logger.LogInformation("Producer Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(3000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }
        }
    }
}
