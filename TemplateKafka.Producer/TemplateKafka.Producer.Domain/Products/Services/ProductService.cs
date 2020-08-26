using Bogus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILoadDataService _loadDataService;
        private readonly IMessageDispatcher _messageDispatcher;
        private IEnumerable<Category> _categories;
        private IEnumerable<Vendor> _vendors;
        private IEnumerable<ProductStatus> _productStatus;

        public ProductService(ILogger<ProductService> logger,
                              IProductRepository productRepository,
                              ILoadDataService loadDataService,
                              IMessageDispatcher messageDispatcher)
        {
            _logger = logger;
            _productRepository = productRepository;
            _loadDataService = loadDataService;
            _messageDispatcher = messageDispatcher;

            LoadCategoriesAndVendorsAndStatus().Wait();
        }

        private async Task LoadCategoriesAndVendorsAndStatus()
        {
            _categories = await _loadDataService.GetCategoriesOrGenerate();

            _vendors = await _loadDataService.GetVendorsOrGenerate();

            _productStatus = await _loadDataService.GetStatus();
        }

        public async Task InsertProducts()
        {
            var product = GenerateProduct(EProductStatus.Created, _categories, _vendors);
            product.Tags = GenerateTags(product);
            _logger.LogInformation($"Create product: {product.Name}");

            if (!await _productRepository.ExistsProduct(product))
            {
                var insertedProduct = await _productRepository.InsertProduct(product);
                _logger.LogInformation($"Save product '{product.Name}' in relational database.");

                if (insertedProduct)
                {
                    product.Category = _categories.First(t => t.Id == product.CategoryId);
                    product.Vendor = _vendors.First(t => t.Id == product.VendorId);
                    product.Status = _productStatus.First(t => t.Id == (int)EProductStatus.Created);

                    PublishToTopic(product);
                }
            }
        }

        public async Task UpdateProducts()
        {
            var productId = await _productRepository.GetRandomProductId();
            if (productId is null)
            {
                return;
            }

            var product = await _productRepository.GetProduct(productId.Value);
            if (product is null)
            {
                return;
            }

            var productChange = GenerateProduct(EProductStatus.Created, _categories, _vendors);
            product.Name = productChange.Name;
            product.Image = productChange.Image;
            product.Price = productChange.Price;
            product.Stock = productChange.Stock;
            product.CategoryId = productChange.CategoryId;
            product.Category = null;
            product.VendorId = productChange.VendorId;
            product.Vendor = null;

            var updatedProduct = await _productRepository.UpdateProduct(product);
            _logger.LogInformation($"Update product '{product.Name}' in relational database.");

            if (updatedProduct)
            {
                product.Category = _categories.First(t => t.Id == product.CategoryId);
                product.Vendor = _vendors.First(t => t.Id == product.VendorId);

                PublishToTopic(product);
            }
        }

        private void PublishToTopic(Product product)
        {
            var productMessage = new ProductDto(product);

            _messageDispatcher.PublishToTopic("Post.Product", new Message<ProductDto>
            {
                CorrelationId = Guid.NewGuid(),
                Data = productMessage
            });

            _logger.LogInformation($"Publish product '{product.Name}' in topic.");
        }

        private Product GenerateProduct(EProductStatus status, IEnumerable<Category> categories, IEnumerable<Vendor> vendors)
            => new Faker<Product>(Locale)
                    .RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                    .RuleFor(c => c.Image, f => $"{f.Image.PlaceImgUrl()}?color={f.Commerce.Color()}")
                    .RuleFor(c => c.Price, f => f.Commerce.Random.Decimal(0.0m, 999.0m))
                    .RuleFor(c => c.Stock, f => f.Commerce.Random.Int(0, 99))
                    .RuleFor(c => c.CategoryId, f => f.PickRandom(categories).Id)
                    .RuleFor(c => c.VendorId, f => f.PickRandom(vendors).Id)
                    .RuleFor(c => c.StatusId, f => (int)status)
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
