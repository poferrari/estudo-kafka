using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Domain.Products.Services
{
    public interface ILoadDataService
    {
        Task<IEnumerable<Category>> GetCategoriesOrGenerate();

        Task<IEnumerable<Vendor>> GetVendorsOrGenerate();
    }
}
