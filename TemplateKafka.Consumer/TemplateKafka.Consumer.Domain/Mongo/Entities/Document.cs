using System;
using TemplateKafka.Consumer.Domain.Mongo.Interfaces;

namespace TemplateKafka.Consumer.Domain.Mongo.Entities
{
    public abstract class Document : IDocument
    {
        public Guid Id { get; set; }
    }
}
