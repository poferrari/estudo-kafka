using System.Threading.Tasks;
using TemplateKafka.Consumer.Domain.Mongo.Interfaces;
using TemplateKafka.Consumer.Domain.Products.Dtos;
using TemplateKafka.Consumer.Domain.Products.Repositories;

namespace TemplateKafka.Consumer.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoRepository<ProductDto> _mongoRepository;

        public ProductRepository(IMongoRepository<ProductDto> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task InsertProduct(ProductDto product)
        {
            await _mongoRepository.InsertOneAsync(product);
        }

        public async Task UpdateProduct(ProductDto product)
        {
            await _mongoRepository.ReplaceOneAsync(product);
        }

        public async Task<ProductDto> GetProduct(ProductDto product)
            => await _mongoRepository.FindOneAsync(t => t.Name.Equals(product.Name) &&
                                                        t.Category.Equals(product.Category) &&
                                                        t.Vendor.Equals(product.Vendor));
    }
}
