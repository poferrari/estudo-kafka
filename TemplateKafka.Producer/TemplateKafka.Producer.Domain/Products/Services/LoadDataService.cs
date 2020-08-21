using Bogus;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Entities;
using TemplateKafka.Producer.Domain.Products.Repositories;

namespace TemplateKafka.Producer.Domain.Products.Services
{
    public class LoadDataService : ILoadDataService
    {
        private const string Locale = "pt_BR";
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVendorRepository _vendorRepository;

        public LoadDataService(ICategoryRepository categoryRepository,
                               IVendorRepository vendorRepository)
        {
            _categoryRepository = categoryRepository;
            _vendorRepository = vendorRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesOrGenerate()
        {
            var categories = await _categoryRepository.GetCategories();

            if (categories is null || !categories.Any())
            {
                categories = GenerateCategories();

                if (!await _categoryRepository.InsertCategories(categories))
                {
                    throw new SqlNullValueException();
                }
            }

            return categories;
        }

        public async Task<IEnumerable<Vendor>> GetVendorsOrGenerate()
        {
            var vendors = await _vendorRepository.GetVendors();

            if (vendors is null || !vendors.Any())
            {
                vendors = GenerateVendors();

                if (!await _vendorRepository.InsertVendors(vendors))
                {
                    throw new SqlNullValueException();
                }
            }

            return vendors;
        }

        private IEnumerable<Category> GenerateCategories()
            => new Faker<Category>(Locale)
                    .RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Commerce.Department())
                    .Generate(15);

        private IEnumerable<Vendor> GenerateVendors()
            => new Faker<Vendor>(Locale)
                    .RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Person.FullName)
                    .Generate(11);
    }
}
