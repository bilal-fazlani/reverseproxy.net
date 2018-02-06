using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ReverseProxy.net
{
    public class ReverseProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ReverseProxySettings _reverseProxySettings;
        //private readonly IConfiguration _configuration;

        public ReverseProxyMiddleware(RequestDelegate next, IOptions<ReverseProxySettings> settings)
        {
            _next = next;
            _reverseProxySettings = settings?.Value;
            //_configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("************* START *********************");

            if (_reverseProxySettings == null || !_reverseProxySettings.Routes.Any())
            {
                await _next(context);
                return;
            }
            
            Console.WriteLine("************* END ***********************");
        }
    }
}