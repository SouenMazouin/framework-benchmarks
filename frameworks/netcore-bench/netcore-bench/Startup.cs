using System.Data.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netcorebench;
using NetCoreBench.Grpc.GrpcImpl;
using NetCoreBench.Grpc.Server;
using NetCoreBench.Models;
using NetCoreBench.Properties;
using Npgsql;

namespace NetCoreBench
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnv)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnv.ContentRootPath)
                .AddJsonFile("Properties/appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(Program.Args);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            // Common DB services
            Configuration.Get<AppSettings>();
            
            var mvcBuilder = services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Latest);
            mvcBuilder.AddJsonFormatters();
            services.AddSingleton<IRandom, DefaultRandom>();
            services.AddSingleton<DbProviderFactory>(NpgsqlFactory.Instance);
            services.AddSingleton<NetCoreBenchService.NetCoreBenchServiceBase, GrpcImpl>();
            services.AddSingleton<GrpcImpl, GrpcImpl>();
            services.Configure<ServiceOptions>(Configuration.GetSection("Service"));
            services.AddSingleton<IDb, DapperDb>();
            services.AddScoped<DapperDb>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}