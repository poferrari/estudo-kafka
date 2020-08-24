using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateKafka.Consumer.Domain.Mongo.Interfaces;
using TemplateKafka.Consumer.Domain.Products.Repositories;
using TemplateKafka.Consumer.Infra.Data.Configs;
using TemplateKafka.Consumer.Infra.Data.Interfaces;
using TemplateKafka.Consumer.Infra.Data.Repositories;

namespace TemplateKafka.Consumer.Infra.IoC.Modules
{
    public static class DataModule
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfig = configuration.GetSection("MongoDbConfig").Get<MongoDbSettings>();
            services.AddSingleton<IMongoDbSettings>(t => mongoConfig);

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
