using E_Commerce.Gateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace E_Commerce.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddAppAuthetication();

            builder.Services.AddOcelot();




            var app = builder.Build();



            app.MapGet("/", () => "Hello World!");

            app.UseOcelot();


            app.Run();
        }
    }
}
