using System.Threading.Tasks;

namespace TemplateKafka.Producer.Domain.Products.Services
{
    public interface IProductService
    {
        Task CreateTopic(string topicName);

        Task InsertProducts();

        Task UpdateProducts();
    }
}
