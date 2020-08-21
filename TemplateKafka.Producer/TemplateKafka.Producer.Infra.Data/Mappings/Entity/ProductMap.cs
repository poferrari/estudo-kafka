using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Infra.Data.Mappings.Entity
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(MapConfig.NameMaxLength)
                   .IsRequired();

            builder.Property(c => c.Image)
                   .HasMaxLength(MapConfig.NameMaxLength);

            builder.Property(c => c.Price)
                   .IsRequired();

            builder.Property(c => c.Stock)
                   .IsRequired();

            builder.HasMany(c => c.Tags)
                   .WithOne(c => c.Product);
        }
    }
}
