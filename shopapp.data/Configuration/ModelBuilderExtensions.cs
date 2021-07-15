using Microsoft.EntityFrameworkCore;
using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace shopapp.data.Configuration
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product() { ProductId = 1, Name = "Huawei P20", Url = "huawei-p20", Price = 2500, ImageUrl = "1.jpg", Description = "32GB 2GB Ram", IsApproved = true },
                new Product() { ProductId = 2, Name = "Huawei P30", Url = "huawei-p30", Price = 3000, ImageUrl = "2.jpg", Description = "64GB 2GB Ram", IsApproved = true },
                new Product() { ProductId = 3, Name = "Huawei P40", Url = "huawei-p40", Price = 3500, ImageUrl = "3.jpg", Description = "128GB 3GB Ram", IsApproved = true },
                new Product() { ProductId = 4, Name = "Huawei Smart", Url = "huawei-smart", Price = 4000, ImageUrl = "4.jpg", Description = "128GB 4GB Ram", IsApproved = true },
               new Product() { ProductId = 5, Name = "Huawei P50", Url = "huawei-p50", Price = 4500, ImageUrl = "1.jpg", Description = "256GB 6GB Ram", IsApproved = true }
           );

            builder.Entity<Category>().HasData(
                new Category() { CategoryId = 1, Name = "Telefon", Url = "telefon" },
                new Category() { CategoryId = 2, Name = "Bilgisayar", Url = "bilgisayar" },
                new Category() { CategoryId = 3, Name = "Elektronik", Url = "elektronik" },
                new Category() { CategoryId = 4, Name = "Beyaz Eşya", Url = "beyaz-esya" }
            );

            builder.Entity<ProductCategory>().HasData(
                new ProductCategory() { ProductId = 1, CategoryId = 1 },
                new ProductCategory() { ProductId = 1, CategoryId = 2 },
                new ProductCategory() { ProductId = 1, CategoryId = 3 },
                new ProductCategory() { ProductId = 2, CategoryId = 1 },
                new ProductCategory() { ProductId = 2, CategoryId = 2 },
                new ProductCategory() { ProductId = 2, CategoryId = 3 },
                new ProductCategory() { ProductId = 3, CategoryId = 4 },
                new ProductCategory() { ProductId = 4, CategoryId = 3 },
                new ProductCategory() { ProductId = 5, CategoryId = 3 },
                new ProductCategory() { ProductId = 5, CategoryId = 1 }
            );
        }
    }
}
