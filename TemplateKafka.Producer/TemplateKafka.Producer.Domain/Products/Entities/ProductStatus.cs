using TemplateKafka.Producer.Domain.Common.Interfaces;

namespace TemplateKafka.Producer.Domain.Products
{
    public class ProductStatus : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
