﻿using System.Threading.Tasks;
using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;

namespace TemplateKafka.Producer.Infra.MessagingBroker.Interfaces
{
    public interface ITopicBroker
    {
        Task Publish<T>(string topicName, Message<T> message);
    }
}
