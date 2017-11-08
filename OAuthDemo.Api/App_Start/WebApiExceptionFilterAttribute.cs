using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using Newtonsoft.Json;
using OAuthDemo.Core.Api.Response;

namespace OAuthDemo.Api
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        /// <summary>
        /// 异常统一处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (actionExecutedContext != null)
                {
                    //响应信息处理
                    var responseMessage = new ResponseBase
                    {
                        Message = actionExecutedContext.Exception.Message,
                        Code = (int)actionExecutedContext.Response.StatusCode
                    };

                    var response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent(JsonConvert.SerializeObject(responseMessage), Encoding.UTF8, "application/json");
                    actionExecutedContext.Response = response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            base.OnException(actionExecutedContext);
        }
    }
}