using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureStorageExample
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(x => Configuration);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            loggerFactory.AddDebug();

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
