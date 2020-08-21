using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Producer.Domain.Products.Services;

namespace TemplateKafka.Producer.Infra.IoC.Modules
{
    public static class DomainModule
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<ILoadDataService, LoadDataService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
