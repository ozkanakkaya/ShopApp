using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace shopapp.data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m => m.ProductId);//birincil anahtar
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.DateAdded).HasDefaultValueSql("getdate()");//boş geçilirse tarihi atar
        }
    }
}
