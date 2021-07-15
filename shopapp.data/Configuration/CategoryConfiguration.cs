using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace shopapp.data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(m => m.CategoryId);//birincil anahtar
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        }
    }
}
