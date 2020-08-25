using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TemplateKafka.Producer.Domain.Products.Repositories;
using TemplateKafka.Producer.Infra.Data.Context;
using TemplateKafka.Producer.Infra.Data.Repositories.Entity;

namespace TemplateKafka.Producer.Infra.IoC.Modules
{
    public static class DataModule
    {
        private const string ConnectionStringName = "DefaultConnection";
        public static readonly ILoggerFactory EFLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });

        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                  options.UseSqlServer(GetConnectionString(configuration))
                         .UseLoggerFactory(EFLoggerFactory),
                  ServiceLifetime.Transient
              );

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductStatusRepository, ProductStatusRepository>();
        }

        private static string GetConnectionString(IConfiguration configuration)
           => configuration.GetConnectionString(ConnectionStringName);
    }
}
