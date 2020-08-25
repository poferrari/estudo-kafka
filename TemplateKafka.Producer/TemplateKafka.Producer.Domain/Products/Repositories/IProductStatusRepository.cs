using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Enums;

namespace TemplateKafka.Producer.Domain.Products.Repositories
{
    public interface IProductStatusRepository
    {
        Task<IEnumerable<ProductStatus>> GetAllStatus();

        Task<ProductStatus> GetStatus(EProductStatus productStatus);
    }
}
