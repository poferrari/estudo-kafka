using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Domain.Products.Repositories
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetVendors();

        Task<Vendor> GetVendor(Guid id);

        Task<bool> InsertVendors(IEnumerable<Vendor> vendors);
    }
}
