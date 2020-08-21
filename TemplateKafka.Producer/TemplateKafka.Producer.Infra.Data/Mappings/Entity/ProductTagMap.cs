using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Infra.Data.Mappings.Entity
{
    public class ProductTagMap : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(MapConfig.NameMaxLength)
                .IsRequired();
        }
    }
}
