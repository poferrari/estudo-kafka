using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Consumer.Infra.MessagingBroker.Brokers;
using TemplateKafka.Consumer.Infra.MessagingBroker.Configs;
using TemplateKafka.Consumer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Consumer.Infra.IoC.Modules
{
    public static class InfrastructureModule
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
            services.AddSingleton(kafkaConfig);

            services.AddSingleton<ITopicConsumer, TopicConsumer>();
        }
    }
}
