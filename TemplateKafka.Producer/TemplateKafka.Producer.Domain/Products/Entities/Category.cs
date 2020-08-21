using System;
using System.Collections.Generic;
using TemplateKafka.Producer.Domain.Common.Interfaces;

namespace TemplateKafka.Producer.Domain.Products.Entities
{
    public class Category : IEntity
    {
        public Category() { }

        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
