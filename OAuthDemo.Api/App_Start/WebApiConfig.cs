using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

namespace OAuthDemo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Web API 开启CORS
            var cors = new EnableCorsAttribute(origins: "*", headers: "*", methods: "*");
            //cors.SupportsCredentials = true;
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Json序列化首字母小谢
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //设置统一Content-Type
            //config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            //统一异常处理
            config.Filters.Add(new WebApiExceptionFilterAttribute());

            ////用户认证
            //config.Filters.Add(new ApiAuthorizeAttribute());
        }
    }
}
