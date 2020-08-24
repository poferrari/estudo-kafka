using System.Threading.Tasks;
using TemplateKafka.Consumer.Domain.Products.Dtos;

namespace TemplateKafka.Consumer.Domain.Products.Repositories
{
    public interface IProductRepository
    {
        Task InsertProduct(ProductDto product);

        Task UpdateProduct(ProductDto product);

        Task<ProductDto> GetProduct(ProductDto product);
    }
}
