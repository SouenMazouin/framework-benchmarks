using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NetCoreBench.Grpc.GrpcImpl;
using NetCoreBench.Grpc.Server;

namespace NetCoreBench
{
    public class Program
    {
        public static string[] Args;

        public static void Main(string[] args)
        {
            Args = args;
            Console.WriteLine();
            Console.WriteLine("ASP.NET Core Benchmarks");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
            Console.WriteLine($"WebHostBuilder loading from: {typeof(WebHostBuilder).GetTypeInfo().Assembly.Location}");
            new ConfigurationBuilder()
                .AddEnvironmentVariables("ASPNETCORE_")
                .AddCommandLine(args)
                .Build();
            BuildWebHost(args)
                .CreateScope()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var webHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("Properties/appsettings.json", false, true);
                })
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(
                    (context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment());
            Console.WriteLine("Press 'G' for GRPC or 'K' for Kestrel :");
            string choice = Console.ReadLine();
            if(choice == "g" || choice == "G")
            {
                webHost.UseGrpc<GrpcImpl>();
            }
            else if (choice == "k" || choice == "K")
            {
                webHost.UseKestrel();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("--- /!\\ --- Press only 'G' or 'K' --- /!\\ --- ");
                Console.WriteLine();
            }
            return webHost.Build();

        }
    }
}