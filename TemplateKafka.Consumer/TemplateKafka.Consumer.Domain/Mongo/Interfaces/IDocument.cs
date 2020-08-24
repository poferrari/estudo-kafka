using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TemplateKafka.Consumer.Domain.Mongo.Interfaces
{
    public interface IDocument
    {
        [BsonId]
        Guid Id { get; set; }
    }
}
