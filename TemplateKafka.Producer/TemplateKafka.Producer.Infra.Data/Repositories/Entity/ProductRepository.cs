using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public async Task<bool> UpdateProduct(Product product)
        {
            _dbSet.Attach(product);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Guid?> GetRandomProductId()
        {
            var random = new Random();
            var guids = await _dbSet.Select(t => t.Id).ToListAsync();
            if (guids is null || !guids.Any())
            {
                return null;
            }
            var index = random.Next(guids.Count);
            return guids[index];
        }

        public async Task<Product> GetProduct(Guid id)
            => await _dbSet.Include(t => t.Category)
                           .Include(t => t.Vendor)
                           .Include(t => t.Status)
                           .Include(t => t.Tags)
                           .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<bool> ExistsProduct(Product product)
            => await _dbSet.FirstOrDefaultAsync(t => t.Name.Equals(product.Name) &&
                                                     t.CategoryId == product.CategoryId &&
                                                     t.VendorId == product.VendorId) != null;
    }
}
