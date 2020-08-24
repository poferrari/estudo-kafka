using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Consumer.Infra.IoC.Modules;

namespace TemplateKafka.Consumer.Infra.IoC
{
    public static class IoC
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            DataModule.Register(services, configuration);
            DomainModule.Register(services);
            InfrastructureModule.Register(services, configuration);
            return services;
        }
    }
}
