using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.Service.Service;
using ClothesStore.WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddOptions();                                         // Kích hoạt Options
            var mailsettings = Configuration.GetSection("MailSettings");  // đọc config
            services.Configure<MailSettings>(mailsettings);                // đăng ký để Inject


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // register service
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IBrandService, BrandService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IColorService, ColorService>();
            services.AddSingleton<IEmployeeService, EmployeeService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<ISizeService, SizeService>();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IOrderService, OrderService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddTransient<IConfigProductService, ConfigProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Detail",
                    "/",
                     new { Controller = "Home", Action = "Index" }
                );
               
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin default",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
