using E_Commerce.ProductAPI.AutoMapperProfiles;
using E_Commerce.ProductAPI.Data;
using E_Commerce.ProductAPI.Extensions;
using E_Commerce.ProductAPI.Services;
using E_Commerce.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
            builder.Services.AddAutoMapper(typeof(CategoryMapper));


            // DEPENDENCY INJECTIONS
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();


            // Serilog Logger

            // dosya isimlendirmesi için özel bir metot yazýlabilir.
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
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id=JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new string[]{}
                    }
                });
            });
            
            builder.AddAppAuthetication();

            builder.Services.AddAuthorization();

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
