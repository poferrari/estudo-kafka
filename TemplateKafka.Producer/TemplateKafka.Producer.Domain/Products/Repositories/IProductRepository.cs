using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Domain.Products.Repositories
{
    public interface IProductRepository
    {
        Task<bool> InsertProduct(Product product);

        Task<bool> ExistsProduct(Product product);
    }
}
