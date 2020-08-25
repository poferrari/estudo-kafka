using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.Data.Context;

namespace TemplateKafka.Producer.Infra.Data.Repositories.Entity
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(DataContext context)
           : base(context) { }

        public async Task<IEnumerable<Vendor>> GetVendors()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<Vendor> GetVendor(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<bool> InsertVendors(IEnumerable<Vendor> vendors)
        {
            await _dbSet.AddRangeAsync(vendors);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
