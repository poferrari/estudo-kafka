using System.Threading.Tasks;

namespace TemplateKafka.Producer.Domain.Products.Services
{
    public interface IProductService
    {
        Task InsertProducts();
    }
}
