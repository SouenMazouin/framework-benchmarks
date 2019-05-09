using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Netcorebench;

namespace NetCoreBench.Grpc.Server
{
    public static class GrpcServerExtentions
    {
        public static IWebHostBuilder UseGrpc<T>(this IWebHostBuilder hostBuilder)
            where T : NetCoreBenchService.NetCoreBenchServiceBase
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, GrpcServer>(provider =>
                {
                    var serverOptions = provider.GetService<IOptions<ServiceOptions>>().Value;
                    var contract = provider.GetService<T>();
                    var serviceDefinition = NetCoreBenchService.BindService(contract);
                    return new GrpcServer(serverOptions.Host, serverOptions.Port, serviceDefinition);
                });
            });
        }

        public static IWebHost CreateScope(this IWebHost webhost)
        {
            //TODO: add creating scope into interceptor
            return webhost;
        }

        public static IWebHost RunGrpcServer(this IWebHost webhost)
        {
            var scope = webhost.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<NetCoreBenchService.NetCoreBenchServiceBase>();
            var serviceOptions = scope.ServiceProvider.GetRequiredService<IOptions<ServiceOptions>>().Value;
            var serviceDefinition = NetCoreBenchService.BindService(service);
            using (var server = new GrpcServer(serviceOptions.Host, serviceOptions.Port, serviceDefinition))
            {
                server.Start();
            }

            return webhost;
        }
    }
}