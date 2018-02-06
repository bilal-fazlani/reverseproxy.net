using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReverseProxy.net
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReverseProxy(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ReverseProxySettings>(configuration.GetSection("ReverseProxy"));
            return services;
        }
    }
}