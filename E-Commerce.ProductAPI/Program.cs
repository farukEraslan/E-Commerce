using E_Commerce.ProductAPI.AutoMapperProfiles;
using E_Commerce.ProductAPI.Data;
using E_Commerce.ProductAPI.Services;
using E_Commerce.ProductAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace E_Commerce.ProductAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // DATABASE CONNECTION
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            // AUTOMAPPER
            builder.Services.AddAutoMapper(typeof(ProductMapper));


            // DEPENDENCY INJECTIONS
            builder.Services.AddScoped<IProductService, ProductService>();


            // Serilog Logger

            // dosya isimlendirmesi i�in �zel bir metot yaz�labilir.
            string logFilePath = $"C:/Users/10132884/OneDrive - NTT DATA Business Solutions AG/Desktop/GitHub/E-CommerceMicroservice/LogFiles/E-Commerce.ProductAPI.Logs/{DateTime.Now.Year}-{DateTime.Now.Month}/.txt";

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File(logFilePath,
                    outputTemplate: "E-Commerce.ProductAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .WriteTo.MSSqlServer("Server = ISTN36002\\SQLEXPRESS; Database = E-Commerce.Logs; uid = sa; pwd = 123; Trusted_Connection = True; TrustServerCertificate = True;",
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "E-Commerce.ProductAPI.Logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            ApplyMigration();

            app.Run();

            void ApplyMigration()
            {
                var scope = app.Services.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (db.Database.GetPendingMigrations().Count() > 0)
                {
                    db.Database.Migrate();
                }
            }
        }
    }
}
