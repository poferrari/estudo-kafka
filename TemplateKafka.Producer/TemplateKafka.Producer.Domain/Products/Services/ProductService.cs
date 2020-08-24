using Bogus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TemplateKafka.Producer.Domain.Products.Dto;
using TemplateKafka.Producer.Domain.Products.Entities;
using TemplateKafka.Producer.Domain.Products.Enums;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.MessagingBroker;
using TemplateKafka.Producer.Infra.MessagingBroker.Brokers;

namespace TemplateKafka.Producer.Domain.Products.Services
{
    public class ProductService : IProductService
    {
        private const string Locale = "pt_BR";
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductStatusRepository _productStatusRepository;
        private readonly ILoadDataService _loadDataService;
        private readonly IMessageDispatcher _messageDispatcher;

        public ProductService(ILogger<ProductService> logger,
                              IProductRepository productRepository,
                              IProductStatusRepository productStatusRepository,
                              ILoadDataService loadDataService,
                              IMessageDispatcher messageDispatcher)
        {
            _logger = logger;
            _productRepository = productRepository;
            _productStatusRepository = productStatusRepository;
            _loadDataService = loadDataService;
            _messageDispatcher = messageDispatcher;
        }

        public async Task InsertProducts()
        {
            var statusCreated = await _productStatusRepository.GetStatus(EProductStatus.Created);

            var categories = await _loadDataService.GetCategoriesOrGenerate();

            var vendors = await _loadDataService.GetVendorsOrGenerate();

            var product = GenerateProduct(statusCreated, categories, vendors);
            product.Tags = GenerateTags(product);
            _logger.LogInformation($"Create product: {product.Name}");

            if (!await _productRepository.ExistsProduct(product))
            {
                var insertedProduct = await _productRepository.InsertProduct(product);
                _logger.LogInformation($"Save product '{product.Name}' in relational database.");

                if (insertedProduct)
                {
                    var productMessage = new ProductDto(product);

                    _messageDispatcher.PublishToTopic("Post.Product", new Message<ProductDto>
                    {
                        CorrelationId = Guid.NewGuid(),
                        Data = productMessage
                    });
                    _logger.LogInformation($"Publish product '{product.Name}' in topic.");

                    Thread.Sleep(2000);
                }
            }
        }

        private Product GenerateProduct(ProductStatus status, IEnumerable<Category> categories, IEnumerable<Vendor> vendors)
            => new Faker<Product>(Locale)
                    .RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                    .RuleFor(c => c.Image, f => f.Image.PlaceImgUrl())
                    .RuleFor(c => c.Price, f => f.Commerce.Random.Decimal(0.0m, 999.0m))
                    .RuleFor(c => c.Stock, f => f.Commerce.Random.Int(0, 99))
                    .RuleFor(c => c.Category, f => f.PickRandom(categories))
                    .RuleFor(c => c.Vendor, f => f.PickRandom(vendors))
                    .RuleFor(c => c.Status, f => status)
                    .Generate();

        private ICollection<ProductTag> GenerateTags(Product product)
        {
            var random = new Random();
            var totalTags = random.Next(2, 10);

            var tags = new Faker<ProductTag>(Locale)
                         .RuleFor(c => c.Id, f => f.Random.Guid())
                         .RuleFor(c => c.Name, f => f.Commerce.ProductAdjective())
                         .RuleFor(c => c.Product, f => product)
                         .Generate(totalTags);

            return tags.GroupBy(tag => tag.Name)
                       .Select(g => g.First()).ToList();
        }
    }
}
