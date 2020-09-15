using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;

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
            //services.AddControllers();
            //services.AddControllers().AddNewtonsoftJson(); // Json Patch
            services.AddControllers().AddNewtonsoftJson().AddXmlSerializerFormatters();  // enabling content negotiation (XML formatting)
            services.Configure<MvcNewtonsoftJsonOptions>(opts => {   // Json Patch
                opts.SerializerSettings.NullValueHandling
                = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            //services.Configure<JsonOptions>(opts => {  //prior to Json Patch
            //    opts.JsonSerializerOptions.IgnoreNullValues = true;
            //});
            services.Configure<MvcOptions>(opts => {        //for Content Negotiation; tells MVC to respect the headers's Accept setting
                opts.RespectBrowserAcceptHeader = true;     //MVC won't now default to Json if the header contains '*/*' in it 
                opts.ReturnHttpNotAcceptable = true;
            });
        }

        public void Configure(IApplicationBuilder app, DataContext context)
        {
            app.UseDeveloperExceptionPage();
            //app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<TestMiddleware>();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/", async context => {
                    await context.Response.WriteAsync("Hello World!");
                });
                //endpoints.MapWebService();
                endpoints.MapControllers();
            });

            SeedData.SeedDatabase(context);
        }
    }
}
