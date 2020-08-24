using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Consumer.Domain.Products.Services;

namespace TemplateKafka.Consumer.Infra.IoC.Modules
{
    public static class DomainModule
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IConsumerProductService, ConsumerProductService>();
        }
    }
}
