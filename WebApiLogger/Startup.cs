﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApiLogger.Data;
using WebApiLogger.Serivces;
using WebApiLogger.Loggers;

namespace WebApiLogger
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add context
            services.AddDbContext<StudentContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add students services
            services.AddScoped<AcademyService>();
            services.AddScoped<IStudentsService, RecruiterService>();          

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(  IApplicationBuilder app, 
                                IHostingEnvironment env, 
                                ILoggerFactory loggerFactory,
                                StudentContext context )
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile("Logs/WebApiLogger-{Date}.txt");

            app.UseMvc();

            DbInitializer.Initialize(context);
        }
    }
}
