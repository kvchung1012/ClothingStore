using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using ClothesStore.Service.Service;
using ClothesStore.WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
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
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
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

            services.AddSingleton<ISliderService, SliderService>();

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
                endpoints.MapControllerRoute(name: "Home",
                    pattern: "/trang-chu",
                    defaults: new { controller = "Home", action = "Index" });
                endpoints.MapControllerRoute(name: "Product",
                    pattern: "/danh-muc",
                    defaults: new { controller = "Product", action = "Index" });

                endpoints.MapControllerRoute(name: "ProductDetail",
                    pattern: "/san-pham/{slug}-{id}",
                    defaults: new { controller = "Product", action = "Detail" });

                endpoints.MapControllerRoute(name: "Cart",
                    pattern: "/don-hang",
                    defaults: new { controller = "Cart", action = "Index" });

                endpoints.MapControllerRoute(name: "Login",
                    pattern: "/dang-nhap",
                    defaults: new { controller = "User", action = "Login" });

                endpoints.MapControllerRoute(name: "Register",
                    pattern: "/dang-ky",
                    defaults: new { controller = "User", action = "Register" });

                endpoints.MapControllerRoute(name: "ForgotPassword",
                    pattern: "/quen-mat-khau",
                    defaults: new { controller = "User", action = "ForgotPassword" });

                endpoints.MapControllerRoute(name: "ChangePassword",
                    pattern: "/doi-mat-khau",
                    defaults: new { controller = "User", action = "ChangePassword" });

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
