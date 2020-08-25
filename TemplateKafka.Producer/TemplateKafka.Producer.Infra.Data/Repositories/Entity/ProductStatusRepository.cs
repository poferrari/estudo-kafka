using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products;
using TemplateKafka.Producer.Domain.Products.Enums;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.Data.Context;

namespace TemplateKafka.Producer.Infra.Data.Repositories.Entity
{
    public class ProductStatusRepository : BaseRepository<ProductStatus>, IProductStatusRepository
    {
        public ProductStatusRepository(DataContext context)
           : base(context) { }

        public async Task<IEnumerable<ProductStatus>> GetAllStatus()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<ProductStatus> GetStatus(EProductStatus productStatus)
            => await _dbSet.FirstOrDefaultAsync(t => t.Id == (int)productStatus);
    }
}
