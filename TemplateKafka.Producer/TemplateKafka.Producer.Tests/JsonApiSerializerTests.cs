using AutoFixture;
using FluentAssertions;
using JsonApiSerializer;
using Newtonsoft.Json;
using NUnit.Framework;
using TemplateKafka.Producer.Domain.Products.Dto;

namespace TemplateKafka.Producer.Tests
{
    public class JsonApiSerializerTests
    {
        private Fixture _fixture;
        private ProductDto _product;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _product = _fixture.Create<ProductDto>();
        }

        [Test]
        public void JsonApiSerializer_ShouldBeSuccessfully()
        {
            var json = JsonConvert.SerializeObject(_product, new JsonApiSerializerSettings());

            json.Should().NotBeNullOrEmpty();
        }

    }
}
