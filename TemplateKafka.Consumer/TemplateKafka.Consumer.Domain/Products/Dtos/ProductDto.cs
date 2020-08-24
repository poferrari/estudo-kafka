using TemplateKafka.Consumer.Domain.Mongo.Attributes;
using TemplateKafka.Consumer.Domain.Mongo.Entities;

namespace TemplateKafka.Consumer.Domain.Products.Dtos
{
    [BsonCollection("product")]
    public class ProductDto : Document
    {
        public string Type { get; } = "product";
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string[] Tags { get; set; }
        public string Category { get; set; }
        public string Vendor { get; set; }
        public string Status { get; set; }
    }
}
