using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using shopapp.business.Abstract;
using shopapp.business.Concrete;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;

namespace shopapp.webui
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, EfCoreProductRepository>();//1. parametre çağırıldığında 2. parametreden nesne üretip gönderir.
            services.AddScoped<IProductService, ProductManager>();

            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddControllersWithViews();//projeye MVC yap�s�n� getirir
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();//wwwroot

            app.UseStaticFiles(new StaticFileOptions
            {//klasöre erişim sağlamak için...
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });

            if (env.IsDevelopment())//ortam değişkenlerine bakar. eğer true dönerse uygulama geliştirme aşamasındayız
            {
                SeedDatabase.Seed();//Oluşturduğumuz fake datadır.
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "adminproductlist",
                    pattern: "admin/products",//bu şekilde bir talep geldiğinde
                    defaults: new { controller = "Admin", action = "ProductList" }//burası çalıştırılacak
                );

                endpoints.MapControllerRoute(
                    name: "adminproductlist",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", action = "Edit" }
                );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Shop", action = "search" }
);

                endpoints.MapControllerRoute(
                    name: "productdetails",//Route ismi
                    pattern: "{url}",//product name url ismidir
                    defaults: new { controller = "Shop", action = "details" }
                    );

                endpoints.MapControllerRoute(
                    name: "products",//Route ismi
                    pattern: "products/{category?}",//product linki/opsiyonel olarak category bilgisi
                    defaults: new { controller = "Shop", action = "list" }
                    );

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
