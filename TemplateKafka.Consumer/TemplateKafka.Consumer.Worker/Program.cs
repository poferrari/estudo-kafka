using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using TemplateKafka.Consumer.Infra.IoC;

namespace TemplateKafka.Consumer.Worker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     var env = ReturnEnvironment(config);
                     config
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile($"appsettings.{(string.IsNullOrEmpty(env) ? "Development" : env)}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.ConfigureContainer(hostContext.Configuration);
                     services.AddHostedService<Worker>();
                 });

        private static string ReturnEnvironment(IConfigurationBuilder configBuilder)
          => configBuilder.SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build()
              .GetSection("Environment")?.Value;
    }
}
