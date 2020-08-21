using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateKafka.Producer.Domain.Products;

namespace TemplateKafka.Producer.Infra.Data.Mappings.Entity
{
    public class ProductStatusMap : IEntityTypeConfiguration<ProductStatus>
    {
        public void Configure(EntityTypeBuilder<ProductStatus> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(MapConfig.NameMaxLength)
                .IsRequired();
        }
    }
}
