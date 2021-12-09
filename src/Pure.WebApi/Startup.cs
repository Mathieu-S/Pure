using System;
using System.IO;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pure.Application.Commons;
using Pure.Application.Handlers;
using Pure.Application.Repositories;
using Pure.Domain.Models;
using Pure.Domain.Validators;
using Pure.Persistence;
using Pure.Persistence.Repositories;
using Serilog;

namespace Pure.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // DbContext
            services.AddDbContext<PureDbContext>(option =>
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PURE_DB")))
                {
                    option.UseNpgsql(Configuration.GetConnectionString("Default"));
                }
                else
                {
                    option.UseNpgsql(Environment.GetEnvironmentVariable("PURE_DB"));
                }
            });

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            // Application Services
            services.AddScoped<IProductHandler, ProductHandler>();
            services.AddScoped<IBrandHandler, BrandHandler>();

            // Domain Services
            services.AddScoped<IValidator<Product>, ProductValidator>();
            services.AddScoped<IValidator<Brand>, BrandValidator>();

            // Swagger config
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pure.WebApi", Version = "v1" });
            
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PureDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pure.WebApi v1"));
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}