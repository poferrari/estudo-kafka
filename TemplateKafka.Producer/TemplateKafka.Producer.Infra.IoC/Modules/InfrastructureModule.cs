using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Producer.Infra.MessagingBroker;
using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;
using TemplateKafka.Producer.Infra.MessagingBroker.Configs;
using TemplateKafka.Producer.Infra.MessagingBroker.Interfaces;

namespace TemplateKafka.Producer.Infra.IoC.Modules
{
    public static class InfrastructureModule
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var kafkaConfig = configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
            services.AddSingleton(kafkaConfig);

            services.AddSingleton<ITopicBroker, TopicBroker>();
            services.AddSingleton<IMessageBuilder, MessageBuilder>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        }
    }
}
