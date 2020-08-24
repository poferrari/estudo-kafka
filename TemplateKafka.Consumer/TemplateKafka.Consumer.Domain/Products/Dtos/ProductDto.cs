using System;

namespace TemplateKafka.Consumer.Domain.Products.Dtos
{
    public class ProductDto
    {
        public string Type { get; } = "product";
        public Guid Id { get; set; }
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
