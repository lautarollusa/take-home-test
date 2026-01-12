using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Fundo.Infrastructure.Data;
using Fundo.Infrastructure.Repositories;
using Fundo.Services;

namespace Fundo.Applications.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FundoDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                    .AddJsonOptions(o =>
                        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<ILoanService, LoanService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Loan Management API",
                    Version = "v1",
                    Description = "RESTful API for managing loans"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular", builder =>
                    builder.WithOrigins("http://localhost:4200", "http://localhost", "https://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FundoDbContext dbContext)
        {
            // Enable Swagger in both Development AND Production
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan API v1");
                c.RoutePrefix = string.Empty; // Swagger at root
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAngular");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
