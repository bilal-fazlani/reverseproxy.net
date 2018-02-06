using System.Collections.Generic;

namespace ReverseProxy.net
{
    public class ReverseProxySettings
    {
        public Dictionary<string, Route> Routes { get; set; }
    }

//    public class Routes
//    {
//        public HashSet<Route> Routes { get; set; }
//    }

    public class Route
    {
        public string Url { get; set; }
//        public string Name { get; set; }

//        public override int GetHashCode()
//        {
//            return Name.GetHashCode();
//        }
//
//        public override bool Equals(object obj)
//        {
//            switch (obj)
//            {
//                case null:
//                    return false;
//                case Route routeSetting:
//                    return routeSetting.Name == Name;
//                default:
//                    return false;
//            }
//        }
    }
}