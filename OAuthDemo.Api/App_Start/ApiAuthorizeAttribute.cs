using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using OAuthDemo.Core.Api.Response;

namespace OAuthDemo.Api
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private string CurrentUserId
        {
            get
            {
                var pricipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                var employeeIdClaim = pricipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return employeeIdClaim?.Value;
            }
        }

        /// <summary>
        /// 检查Action和对应的控制器是否匿名访问
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //可以匿名访问或者当前用户已登录
            if (SkipAuthorization(actionContext)) base.OnAuthorization(actionContext);
            //获取到用户信息
            if (!string.IsNullOrEmpty(CurrentUserId)) base.IsAuthorized(actionContext);
            if (!SkipAuthorization(actionContext) && string.IsNullOrEmpty(CurrentUserId))
            {
                var response = new ResponseBase { Code = 401, Message = "需要授权访问" };
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json")
                };
            }
        }
    }
}