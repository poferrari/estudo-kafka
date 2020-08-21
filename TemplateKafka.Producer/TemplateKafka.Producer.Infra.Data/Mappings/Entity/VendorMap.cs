using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateKafka.Producer.Domain.Products.Entities;

namespace TemplateKafka.Producer.Infra.Data.Mappings.Entity
{
    public class VendorMap : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(MapConfig.NameMaxLength)
                .IsRequired();

            builder.HasMany(c => c.Products)
                   .WithOne(c => c.Vendor);
        }
    }
}
