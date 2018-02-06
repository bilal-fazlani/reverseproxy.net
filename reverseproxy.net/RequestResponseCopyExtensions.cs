using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ReverseProxy.net
{
    public static class RequestResponseCopyExtensions
    {
        public static async Task<HttpWebRequest> CopyAsync(this HttpRequest original, string newUrl)
        {
            HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(newUrl);

            //body
            if(original.Method != "GET")
                await original.Body.CopyToAsync(newRequest.GetRequestStream());

            //copy headers
            foreach (var header in original.Headers)
            {
                if (!WebHeaderCollection.IsRestricted(header.Key))
                {
                    newRequest.Headers[header.Key] = original.Headers[header.Key];
                }
            }
            
            // Copy restricted headers
            if (original.GetTypedHeaders().Accept.Any())
            {
                newRequest.Accept = string.Join(",", original.GetTypedHeaders().Accept.ToString());
            }
            
            newRequest.Method = original.Method;
            newRequest.UserAgent = original.Headers["User-Agent"].ToString();
            newRequest.ContentType = original.ContentType;
            newRequest.Referer = original.Headers["Referer"].ToString();
            
            //copy cookies
            foreach (var cookie in original.Cookies)
            {
                newRequest.CookieContainer.Add(new Cookie(cookie.Key, cookie.Value));
            }
            
            return newRequest;
        }
        
        public static async Task CopyAsyncTo(this WebException original, HttpResponse target)
        {
            //body
            if (original.Response != null)
            {
                //content type
                target.ContentType = original.Response.ContentType;

                //status
                if (original.Status == WebExceptionStatus.ProtocolError)
                {
                    target.StatusCode = (int) (((HttpWebResponse) original.Response).StatusCode);
                }
                else
                {
                    target.StatusCode = (int) HttpStatusCode.BadGateway;
                }
                
                await original.Response.GetResponseStream().CopyToAsync(target.Body);
            }
            else
            {
                //status
                target.StatusCode = (int) HttpStatusCode.BadGateway;
                
                //content type
                target.ContentType = "application/json";
                
                //body
                using (StreamWriter sw = new StreamWriter(target.Body))
                {
                    await sw.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        error = original.Status.ToString()
                    }));
                }
            }
        }

        public static async Task CopyAsyncTo(this WebResponse original, HttpResponse target)
        {
            //content type
            target.ContentType = original?.ContentType;

            if (original is HttpWebResponse originalWebResponse)
            {
                //headers
                foreach (string headerKey in originalWebResponse.Headers.Keys)
                {
                    target.Headers[headerKey] = originalWebResponse.Headers[headerKey];
                }

                //status
                target.StatusCode = (int) originalWebResponse.StatusCode;
                
                //cookies
                foreach (Cookie cookie in originalWebResponse.Cookies)
                {
                    target.Cookies.Append(cookie.Name, cookie.Value, new CookieOptions
                    {
                        Domain = cookie.Domain,
                        Expires = cookie.Expires,
                        HttpOnly = cookie.HttpOnly,
                        Path = cookie.Path,
                        Secure = cookie.Secure
                    });
                }
            }
            
            //content length
            target.ContentLength = original?.ContentLength;
          
            //body
            await original.GetResponseStream().CopyToAsync(target.Body);
        }
        
        public static async Task AddErrorAsync(this HttpResponse target, object body)
        {
            //content type
            target.ContentType = "application/json";
            
            //status code
            target.StatusCode = (int) HttpStatusCode.BadGateway;
            
            //body
            using (StreamWriter sw = new StreamWriter(target.Body))
            {
                await sw.WriteAsync(JsonConvert.SerializeObject(body));
            }
        }

        public static HttpWebRequest AddBearerToken(this HttpWebRequest request, string token)
        {
            request.Headers.Add("Authorization", "Bearer " + token);
            return request;
        }
    }
}