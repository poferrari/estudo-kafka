using System;
using System.Collections.Generic;
using TemplateKafka.Producer.Domain.Common.Interfaces;

namespace TemplateKafka.Producer.Domain.Products.Entities
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public ICollection<ProductTag> Tags { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public int StatusId { get; set; }
        public ProductStatus Status { get; set; }
    }
}
