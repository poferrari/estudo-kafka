using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Enums;

namespace TemplateKafka.Producer.Domain.Products.Repositories
{
    public interface IProductStatusRepository
    {
        Task<ProductStatus> GetStatus(EProductStatus productStatus);
    }
}
