namespace TemplateKafka.Consumer.Infra.MessagingBroker.Configs
{
    public class KafkaConfig
    {
        public string GroupId { get; set; }
        public string Brokers { get; set; }
        public int Timeout { get; set; }
        public string ProductTopic { get; set; }
    }
}
