using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.Data.Context;

namespace TemplateKafka.Producer.Infra.Data.Repositories.Entity
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context)
           : base(context) { }

        public async Task<bool> InsertProduct(Product product)
        {
            await _dbSet.AddAsync(product);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsProduct(Product product)
            => await _dbSet.FirstOrDefaultAsync(t => t.Name.Equals(product.Name) &&
                                                     t.Category.Id == product.Category.Id &&
                                                     t.Vendor.Id == product.Vendor.Id) != null;
    }
}
