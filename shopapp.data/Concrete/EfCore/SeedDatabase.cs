using Microsoft.EntityFrameworkCore;
using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shopapp.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)//Migration sayısı 0 mı?
            {
                if (context.Categories.Count() == 0)//kategori sayısı?
                {
                    context.Categories.AddRange(Categories);//kategori listesi eklenir
                }

                if (context.Products.Count() == 0)//ürünler sayısı?
                {
                    context.Products.AddRange(Products);//ürünlerin listesi eklenir
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},
            new Category(){Name="Bilgisayar"},
            new Category(){Name="Elektronik"}
        };

        private static Product[] Products =
        {
            new Product(){Name="Huawei P20",Price=2500,ImageUrl="1.jpg",Description="32GB 2GB Ram",IsApproved=true},
            new Product(){Name="Huawei P30",Price=3000,ImageUrl="2.jpg",Description="64GB 2GB Ram",IsApproved=true},
            new Product(){Name="Huawei P40",Price=3500,ImageUrl="3.jpg",Description="128GB           3GBRam",IsApproved=true},
            new Product(){Name="Huawei Smart",Price=4000,ImageUrl="4.jpg",Description="128GB         4GBRam",IsApproved=true},
           new Product(){Name="Huawei P50",Price=4500,ImageUrl="1.jpg",Description="256GB        6GBRam",IsApproved=true}
        };
    }
}
