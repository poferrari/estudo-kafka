using System;
using System.Linq;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Domain.Products.Dto
{
    public class ProductDto
    {
        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Image = product.Image;
            Price = product.Price;
            Stock = product.Stock;
            Tags = product.Tags.Select(t => t.Name).ToArray();
            Category = product.Category.Name;
            Vendor = product.Vendor.Name;
            Status = product.Status.Name;
        }

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
