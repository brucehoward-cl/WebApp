using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
//using Microsoft.AspNetCore.Razor.TagHelpers; //for registering tag helpers
//using WebApp.TagHelpers;  //for registering tag helpers
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;


namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
        
        public IConfiguration Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opts => {
                opts.UseSqlServer(Configuration["ConnectionStrings:ProductConnection"]);
                opts.EnableSensitiveDataLogging(true);
            });
        
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddSingleton<CitiesData>();
            //services.AddTransient<ITagHelperComponent, TimeTagHelperComponent>();  //registers the tag helpers as a service; AddTransient ensures each request is handled using its own instance
            //services.AddTransient<ITagHelperComponent, TableFooterTagHelperComponent>(); //notice that each defined TagHelperComponent must be registered separately
            services.Configure<AntiforgeryOptions>(opts => {
                opts.HeaderName = "X-XSRF-TOKEN";
            });
            services.Configure<MvcOptions>(opts => 
                opts.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(value => "Please enter a value"));
            services.AddScoped<GuidResponseAttribute>();
            services.Configure<MvcOptions>(opts => {    //MvcOptions.Filters property returns collection to which filters are added to apply them globally
                opts.Filters.Add<HttpsOnlyAttribute>();     //There is also a non-generic Add() method
                opts.Filters.Add(new MessageAttribute("This is the globally-scoped filter"));   //this global result filter is added
            });
        }

        //public void Configure(IApplicationBuilder app, DataContext context)
        public void Configure(IApplicationBuilder app, DataContext context, IAntiforgery antiforgery)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.Use(async (context, next) => {
                if (!context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.Cookies.Append("XSRF-TOKEN", antiforgery.GetAndStoreTokens(context).RequestToken, new CookieOptions { HttpOnly = false });
                }
                await next();
            }); //To enable the antiforgery token for use with javascript
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("forms", "controllers/{controller=Home}/{action=Index}/{id?}"); //allows better differentiation between controllers and pages in examples
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
            SeedData.SeedDatabase(context);
        }
    }
}

#region Chapter 24 View Components
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using WebApp.Models;
//using Microsoft.AspNetCore.Mvc.RazorPages;


//namespace WebApp
//{
//    public class Startup
//    {
//        public Startup(IConfiguration config)
//        {
//            Configuration = config;
//        }

//        public IConfiguration Configuration { get; set; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddDbContext<DataContext>(opts => {
//                opts.UseSqlServer(Configuration["ConnectionStrings:ProductConnection"]);
//                opts.EnableSensitiveDataLogging(true);
//            });

//            //services.AddControllers();
//            services.AddControllersWithViews().AddRazorRuntimeCompilation();
//            services.AddRazorPages().AddRazorRuntimeCompilation();  //needed to add razor pages (enables runtime recompilation)
//            services.AddDistributedMemoryCache();

//            services.AddSession(options => {
//                options.Cookie.IsEssential = true;
//            });

//            services.Configure<RazorPagesOptions>(opts => {
//                opts.Conventions.AddPageRoute("/Index", "/extra/page/{id:long?}");
//            });

//            services.AddSingleton<CitiesData>();
//        }

//        public void Configure(IApplicationBuilder app, DataContext context)
//        {
//            app.UseDeveloperExceptionPage();
//            app.UseStaticFiles();
//            app.UseSession();
//            app.UseRouting();
//            app.UseEndpoints(endpoints => {
//                endpoints.MapControllers();
//                //endpoints.MapControllerRoute("Default","{controller=Home}/{action=Index}/{id?}"); //this is so common it can be replaced with the MapDefaultControllerRoute() config
//                endpoints.MapDefaultControllerRoute();
//                endpoints.MapRazorPages();  //needed to add razor pages (creates the routing configuration)
//            });

//            SeedData.SeedDatabase(context);
//        }
//    }
//} 
#endregion

#region Prior to Chapter 21
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using WebApp.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.OpenApi.Models; 
#endregion

#region Prior to Chapter 21
//public class Startup
//{
//    public Startup(IConfiguration config)
//    {
//        Configuration = config;
//    }

//    public IConfiguration Configuration { get; set; }

//    public void ConfigureServices(IServiceCollection services)
//    {
//        services.AddDbContext<DataContext>(opts =>
//        {
//            opts.UseSqlServer(Configuration["ConnectionStrings:ProductConnection"]);
//            opts.EnableSensitiveDataLogging(true);
//        });
//        //services.AddControllers();
//        //services.AddControllers().AddNewtonsoftJson(); // Json Patch
//        services.AddControllers().AddNewtonsoftJson().AddXmlSerializerFormatters();  // enabling content negotiation (XML formatting)
//        services.Configure<MvcNewtonsoftJsonOptions>(opts =>
//        {   // Json Patch
//            opts.SerializerSettings.NullValueHandling
//            = Newtonsoft.Json.NullValueHandling.Ignore;
//        });
//        //services.Configure<JsonOptions>(opts => {  //prior to Json Patch
//        //    opts.JsonSerializerOptions.IgnoreNullValues = true;
//        //});
//        services.Configure<MvcOptions>(opts =>
//        {        //for Content Negotiation; tells MVC to respect the headers's Accept setting
//            opts.RespectBrowserAcceptHeader = true;     //MVC won't now default to Json if the header contains '*/*' in it 
//            opts.ReturnHttpNotAcceptable = true;
//        });
//        services.AddSwaggerGen(options =>
//        {             //added for Documenting Web Services lesson
//            options.SwaggerDoc("v1",
//            new OpenApiInfo { Title = "WebApp", Version = "v1" });
//        });
//    }

//    public void Configure(IApplicationBuilder app, DataContext context)
//    {
//        app.UseDeveloperExceptionPage();
//        //app.UseStaticFiles();
//        app.UseRouting();
//        app.UseMiddleware<TestMiddleware>();
//        app.UseEndpoints(endpoints =>
//        {
//            endpoints.MapGet("/", async context =>
//            {
//                await context.Response.WriteAsync("Hello World!");
//            });
//            //endpoints.MapWebService();
//            endpoints.MapControllers();
//        });

//        #region Documenting Web Services
//        app.UseSwagger();
//        app.UseSwaggerUI(options =>
//        {
//            options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
//        });
//        #endregion

//        SeedData.SeedDatabase(context);
//    }
//} 
#endregion
