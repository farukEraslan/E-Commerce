using E_Commerce.AuthAPI.AutoMapperProfiles;
using E_Commerce.AuthAPI.Data;
using E_Commerce.AuthAPI.Models;
using E_Commerce.AuthAPI.Services;
using E_Commerce.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace E_Commerce.AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 

            // JWT OPTIONS SETTING
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

            // DATABASE CONNECTION
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlServer(builder.Configuration.GetConnectionString("DockerSqlConnection"));
            });

            // IDENTITY
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // AUTOMAPPER
            builder.Services.AddAutoMapper(typeof(MapperProfiles));


            // DEPENDENCY INJECTIONS
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // Serilog Logger
            
            // dosya isimlendirmesi için özel bir metot yazýlabilir.
            //string logFilePath = $"C:/Users/10132884/OneDrive - NTT DATA Business Solutions AG/Desktop/GitHub/E-CommerceMicroservice/LogFiles/E-Commerce.AuthAPI.Logs/{DateTime.Now.Year}-{DateTime.Now.Month}/.txt";
            string logFilePath = $"D:/GitHub Projects/E-Commerce/LogFiles/LogFiles/E-Commerce.AuthAPI.Logs/{DateTime.Now.Year}-{DateTime.Now.Month}/.txt";
            string connectionString = "Server = FARUKERASLAN\\FARUKERASLAN; Database = E-Commerce.Logs; uid = sa; pwd = 123; Trusted_Connection = True; TrustServerCertificate = True;";

            #region Database Loglama
            /*         
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.File(logFilePath,
                    outputTemplate: "E-Commerce.AuthAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .WriteTo.MSSqlServer(connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "E-Commerce.AuthAPI.Logs",
                        AutoCreateSqlTable = true
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information).CreateLogger();
            */
            #endregion
            
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.File(logFilePath,
                    outputTemplate: "E-Commerce.AuthAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .CreateLogger();

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
