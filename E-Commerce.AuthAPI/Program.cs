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
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // IDENTITY
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // AUTOMAPPER
            builder.Services.AddAutoMapper(typeof(MapperProfiles));


            // DEPENDENCY INJECTIONS
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            // Serilog Logger
            string logFilePath = "C:\\Users\\10132884\\OneDrive - NTT DATA Business Solutions AG\\Desktop\\GitHub\\E-CommerceMicroservice";

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("LogFiles/E-Commerce.AuthAPI.Logs.txt",
                    outputTemplate: "E-Commerce.AuthAPI | {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .WriteTo.MSSqlServer("Server = ISTN36002\\SQLEXPRESS; Database = E-Commerce.Logs; uid = sa; pwd = 123; Trusted_Connection = True; TrustServerCertificate = True;",
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "E-Commerce.AuthAPI.Logs",
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
