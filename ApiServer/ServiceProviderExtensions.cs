using Serilog;
using Database.Contexts.Models;
using Database.Context;
using Microsoft.EntityFrameworkCore;
using MobileDrill.Services.RepositoryFolder;
using Database.Contexts.Services;

namespace ApiServer
{
    public static class ServiceProviderExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration Configuration)
        {
            AddTransientServices(services,Configuration);

            services.AddDbContext<IDbContext, MyDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);


            services.AddSignalR(options => options.EnableDetailedErrors = true)
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });


            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin();
                // builder.AllowAnyMethod()
                // .AllowAnyHeader()
                // .AllowCredentials()
                // .SetIsOriginAllowed(_ => true)
                // .WithOrigins("http://localhost:4200/", "http://localhost:8080/");
            }));

            using var serviceProvider = services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<MyDbContext>();
            context.Database.EnsureCreated();
        }

        private static void AddTransientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<DbContextService>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
        private static void AddScopedServices(this IServiceCollection services, IConfiguration Configuration)
        {

        }
        private static void AddSingeltonServices(this IServiceCollection services, IConfiguration Configuration)
        {

        }
        private static void AddHostedService(this IServiceCollection services, IConfiguration Configuration)
        {

        }

        public static void InitLogging(this IServiceCollection services, IConfiguration Configuration)
        {
            var logLevel = Serilog.Events.LogEventLevel.Error;

            string logsFolder = "logs";

            string loggerOutputTemplate =
                "-------------------------------------\n[{ProjectSignature}] {Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{FilePath}/{MemberName}:{LineNumber}]\n[{Tag}][{Level:u3}]: {Message:lj}{NewLine}{Exception}";
            string currentDate = DateTime.Today.ToString("dd.MM.yyyy");

            Serilog.Log.Logger = new Serilog.LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", logLevel)
                .WriteTo.Console(outputTemplate: loggerOutputTemplate)
                .WriteTo.Map("FileName", "main", (name, wt) =>
                {
                    wt.File(path: $"{logsFolder}/{currentDate}/{name}.log", outputTemplate: loggerOutputTemplate);
                })
                .CreateLogger();
        }

    }
}
