using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EFCore.Platforms.Models;

namespace EFCore.Platforms
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var dbConnectionString = Configuration["DataAccess:PostgreSqlProvider:ConnectionString"];
            //services.AddDbContext<StarWarsContext>(options =>
            //    options.UseNpgsql(dbConnectionString));

            var dbConnectionString = Configuration["DataAccess:SqlServerProvider:ConnectionString"];
            services.AddDbContext<StarWarsContext>(options =>
                options.UseSqlServer(dbConnectionString));

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.ApplicationServices.GetService<StarWarsContext>().Database.EnsureCreated();
            app.ApplicationServices.GetService<StarWarsContext>().EnsureSeedData();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
           
            app.UseMvc();
        }
    }
}
