using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Cache;
using TAM.VMS.Infrastructure.Helper;
using TAM.VMS.Infrastructure.Web.Memory;
using TAM.VMS.Service;
using TAM.VMS.Web.Filters;

namespace TAM.VMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            // Set global cache values


            using (var Db = new DbHelper())
            {
                var config = Db.ConfigRepository.FindAll();
                var generalCategory = Db.GeneralCategoryRepository.FindAll();
                var rolePermission = Db.RolePermissionRepository.FindAll();
                var menu = Db.MenuRepository.FindAll();
                ApplicationCacheManager.Set(ApplicationCacheManager.AppConfigCacheKey, config.ToList());
                ApplicationCacheManager.Set(ApplicationCacheManager.GeneralCategoryCacheKey, generalCategory.ToList());
                ApplicationCacheManager.Set(ApplicationCacheManager.PermissionCacheKey, rolePermission.ToList());
                ApplicationCacheManager.Set(ApplicationCacheManager.PermissionMenuCacheKey, menu.ToList());
            }
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable runtime compilation
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddControllersWithViews();

            services.AddMvc(o => { o.Filters.Add<ExceptionFilter>(); })
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.PropertyNamingPolicy = null; });

            //Configure cookie policy
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.Secure = Environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
            });

            //services.AddMvc(options => {
            //    options.Filters.Add(new AuthorizeActionFilter());
            //});

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddMvc(options =>
            {
                options.MaxModelBindingCollectionSize = 100000;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Core/Account/Login");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Core/Account/Logout");
                });

            //Register Memory Cache
            services.AddMemoryCache();

            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(Configuration["Authentication:ExpireTimeSpan"]));
            });

            //Add service for accessing current HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddScoped(it => it.GetService<IUrlHelperFactory>()
                             .GetUrlHelper(it.GetService<IActionContextAccessor>().ActionContext));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                var maintenance = bool.Parse(Configuration["Maintenance"]);

                options.Cookie.Name = "TAM.VMS" + (Environment.IsDevelopment() ? ".DEV" : string.Empty);
                options.Cookie.HttpOnly = bool.Parse(Configuration["Authentication:HttpOnly"]);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(Configuration["Authentication:ExpireTimeSpan"]) - 1);
                options.SlidingExpiration = bool.Parse(Configuration["Authentication:SlidingExpiration"]);

                var path = Configuration[maintenance ? "MaintenanceHandler" : "Authentication:LoginPath"];

                options.LoginPath = path;
                options.LogoutPath = Configuration["Authentication:LogoutPath"];
                options.AccessDeniedPath = options.LoginPath;
                options.ReturnUrlParameter = Configuration["Authentication:ReturnUrlParameter"];

                var useMemoryCacheSessionStore = bool.Parse(Configuration["Authentication:MemoryCacheSessionStore"]);

                if (useMemoryCacheSessionStore)
                {
                    options.SessionStore = new MemoryCacheTicketStore(options);
                }
            });
            // Add Kendo UI services to the services container
            services.AddKendo();


            services.AddScoped<IDbHelper>(options =>
            {
                return new DbHelper();
            });

            services.Scan(scanner => scanner.FromAssemblyOf<DbService>()
                .AddClasses(@class => @class.Where(type => type.Name.EndsWith("Service")))
                .AsSelf()
                .WithTransientLifetime()
            );

#if DEBUG
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });
#else
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var useCookiePolicy = bool.Parse(Configuration["UseCookiePolicy"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else app.UseExceptionHandler("/Error?statusCode=500");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {

                    context.Context.Response.Headers.Add("cache-control", new[] { "public,max-age=31536000" });
                    context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123

                }
            });
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseSession();
            Agit.Framework.Web.HttpContext.Services = app.ApplicationServices;
            app.UseHttpContext();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area=Core}/{controller=Default}/{action=Index}");
                endpoints.MapControllers();
            });
        }
    }

}
