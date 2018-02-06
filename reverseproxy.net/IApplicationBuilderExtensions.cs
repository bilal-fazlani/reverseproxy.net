using Microsoft.AspNetCore.Builder;

namespace ReverseProxy.net
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseReverseProxy(this IApplicationBuilder app)
        {
            app.UseMiddleware<ReverseProxyMiddleware>();
            return app;
        }
    }
}