using System.Collections.Generic;

namespace ReverseProxy.net
{
    public class ReverseProxySettings
    {
        public Dictionary<string, Route> Routes { get; set; }
    }
    
    public class Route
    {
        public string Url { get; set; }
    }
}