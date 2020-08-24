using TemplateKafka.Consumer.Infra.Data.Interfaces;

namespace TemplateKafka.Consumer.Infra.Data.Configs
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
