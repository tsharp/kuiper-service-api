using Kuiper.Clustering.ServiceApi.Storage;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Kuiper.Clustering.ServiceApi
{
    public class Program
    {
        static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddResourceHandlers();
            builder.Services.AddScoped<IKeyValueStore, KvStoreDbContext>();

            builder.Services.AddDbContext<KvStoreDbContext>(options =>
            {
                // options.UseInMemoryDatabase("kvstore");
                
                // Get application data directory
                var application_data = Environment.SpecialFolder.ApplicationData;
                
                // Create a directory for the application data
                string application_data_path = Path.Combine(Environment.GetFolderPath(application_data), "kuiper_data");

                if (IsLinux())
                {
                    application_data_path = "/var/lib/kuiperdb";
                }

                var system_db_path = Path.Combine(application_data_path, "system.db");

                Console.WriteLine($"Database Path: {system_db_path}");

                if (!Directory.Exists(application_data_path))
                {
                    Console.WriteLine("Creating data directory ...");
                    Directory.CreateDirectory(application_data_path);
                }

                // Configure the database to use SQLite
                options.UseSqlite($"Data Source={system_db_path}");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.Map("/api/{*fullPath}", async (HttpContext httpContext, string fullPath) =>
            {
                var descriptor = ResourcePathParser.Parse(fullPath);
                var handler = httpContext.RequestServices.ResolveResourceHandler
                    (descriptor.Api, descriptor.Version, descriptor.ResourceType);

                descriptor.HandlerType = handler.GetType();

                return await handler.HandleRequest(httpContext, descriptor);
            });

            app.Run();
        }
    }
}
