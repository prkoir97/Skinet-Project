using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // builder.Property() call configures a specific property of the Product entity
            // You can set various constraints such as requiredness, maximum length, and data type using these configurations
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).IsRequired();

            // A Product belongs to one ProductBrand (HasOne), and a ProductBrand can have many Products (WithMany)
            // HasForeignKey() method specifies which property in the Product entity is the foreign key for the relationship with the related entity
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(t => t.ProductType).WithMany()        
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}