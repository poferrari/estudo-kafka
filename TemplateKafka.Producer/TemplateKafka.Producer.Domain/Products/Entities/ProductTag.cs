using System;
using TemplateKafka.Producer.Domain.Common.Interfaces;

namespace TemplateKafka.Producer.Domain.Products.Entities
{
    public class ProductTag : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }
    }
}
