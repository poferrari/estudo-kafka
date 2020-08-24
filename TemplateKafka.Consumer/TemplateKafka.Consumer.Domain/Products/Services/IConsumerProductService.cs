using System.Threading;

namespace TemplateKafka.Consumer.Domain.Products.Services
{
    public interface IConsumerProductService
    {
        void Subscribe(CancellationTokenSource cancellationTokenSource);
    }
}
