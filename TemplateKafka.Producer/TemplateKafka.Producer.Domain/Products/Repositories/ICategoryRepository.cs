using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Domain.Products.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<bool> InsertCategories(IEnumerable<Category> categories);
    }
}
