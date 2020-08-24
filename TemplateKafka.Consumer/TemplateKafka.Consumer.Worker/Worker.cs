using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TemplateKafka.Consumer.Domain.Products.Services;

namespace TemplateKafka.Consumer.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger,
                      IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var productService = scope.ServiceProvider.GetRequiredService<IConsumerProductService>();

            var cancellationTokenSource = new CancellationTokenSource();
            productService.Subscribe(cancellationTokenSource);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Consumer Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
