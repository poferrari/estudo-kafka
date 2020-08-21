using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Producer.Infra.IoC.Modules;

namespace TemplateKafka.Producer.Infra.IoC
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
