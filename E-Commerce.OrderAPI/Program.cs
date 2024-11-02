using E_Commerce.OrderAPI.AutoMapperProfiles;
using E_Commerce.OrderAPI.Data;
using E_Commerce.OrderAPI.Extensions;
using E_Commerce.OrderAPI.Services;
using E_Commerce.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace E_Commerce.OrderAPI
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
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlServer(builder.Configuration.GetConnectionString("DockerSqlConnection"));
            });


            // AUTOMAPPER
            builder.Services.AddAutoMapper(typeof(CartMapper));
            builder.Services.AddAutoMapper(typeof(CartLineMapper));



            // DEPENDENCY INJECTIONS
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IUserService, UserService>();
            

            builder.Services.AddHttpClient("Product", p => p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));
            builder.Services.AddHttpClient("Order", p => p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:OrderAPI"]));
            builder.Services.AddHttpClient("User", p => p.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AuthAPI"]));



            // Serilog Logger

            // dosya isimlendirmesi için özel bir metot yazýlabilir.
            string logFilePath = $"D:/GitHub Projects/E-Commerce/LogFiles/E-Commerce.OrderAPI.Logs/{DateTime.Now.Year}-{DateTime.Now.Month}/.txt";

            #region Database Loglama
            /*
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File(logFilePath,
                    outputTemplate: "E-Commerce.OrderAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .WriteTo.MSSqlServer("Server = FARUKERASLAN\\FARUKERASLAN; Database = E-Commerce.Logs; uid = sa; pwd = 123; Trusted_Connection = True; TrustServerCertificate = True;",
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "E-Commerce.OrderAPI.Logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();
            */
            #endregion

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.File(logFilePath,
                   outputTemplate: "E-Commerce.OrderAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                   rollingInterval: RollingInterval.Day,
                   retainedFileCountLimit: null)
                .CreateLogger();

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
