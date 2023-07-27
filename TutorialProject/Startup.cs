using BusinessLayer;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VoteApi;

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
            // Override default layout path
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // Replace "~/Views/Shared/AppLayout" with the path to your custom layout folder.
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("~/Views/Shared/AppLayout/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("~/Views/Shared/AppLayout/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Identity manager
            services.AddIdentity<AppUser, IdentityRole>(_ =>
            {
                _.Password.RequiredLength = 6; //En az kaç karakterli olmasý gerektiðini belirtiyoruz.
                _.Password.RequireNonAlphanumeric = false;//Alfanumerik zorunluluðunu kaldýrýyoruz.
                _.Password.RequireLowercase = false;//Küçük harf zorunluluðunu kaldýrýyoruz.
                _.Password.RequireUppercase = false;//Büyük harf zorunluluðunu kaldýrýyoruz.
                _.Password.RequireDigit = false;//0-9 arasý sayýsal karakter zorunluluðunu kaldýrýyoruz.
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
            });

            // Mongo db config
            services.Configure<VoteDbSettings>(Configuration.GetSection(nameof(VoteDbSettings)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<VoteDbSettings>>().Value);

            services.AddControllersWithViews();
            services.AddTransient<Context>();
            services.AddTransient<ThreadService>();
            services.AddTransient<VoteService>(); // maybe singleton idk
            services.AddScoped<ThreadDal>();
            services.AddScoped<UserDal>();
            services.AddScoped<CategoryDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
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

            // Ensure the "admin" role exists and create if not
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.CreateAsync(adminRole).Wait();
            }

            // Ensure the "normal" role exists and create if not
            if (!roleManager.RoleExistsAsync("Normal").Result)
            {
                var normalRole = new IdentityRole("Normal");
                roleManager.CreateAsync(normalRole).Wait();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Threads List",
                    pattern: "Comments",
                    defaults: new { controller = "Thread", action = "Comments" }
                    );
                endpoints.MapControllerRoute(
                 name: "Threads List",
                 pattern: "Vote",
                 defaults: new { controller = "Thread", action = "Vote" }
                 );
                endpoints.MapControllerRoute(
                    name: "Threads List",
                    pattern: "Comments/{id}",
                    defaults: new { controller = "Thread", action = "Comments" }
                    );
                endpoints.MapControllerRoute(
                    name: "Threads List",
                    pattern: "",
                    defaults: new { controller = "Thread", action = "List" }
                    );
                endpoints.MapControllerRoute(
                    name: "Thread Details",
                    pattern: "Thread/{id?}",
                    defaults: new { controller = "Thread", action = "Details" }
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Error}/{action=Index}/{id?}");
            });
        }
    }
}
