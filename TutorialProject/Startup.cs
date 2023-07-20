using BusinessLayer;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TutorialProject
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
            services.AddIdentity<IdentityUser, IdentityRole>()
       .AddEntityFrameworkStores<Context>()
       .AddDefaultTokenProviders();

            // Configure authentication options
            services.Configure<IdentityOptions>(options =>
            {
                // Customize your authentication settings if needed
            });
            services.AddControllersWithViews();
            services.AddTransient<Context>();
            services.AddTransient<AuthService>();
            services.AddScoped<ThreadDal>();
            services.AddScoped<UserDal>();
            services.AddScoped<CategoryDal>();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Thread",
                    pattern: "Thread/{id?}",
                    defaults: new { controller = "Thread", action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Error}/{action=Index}/{id?}");
            });
        }
    }
}
