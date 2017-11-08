using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using OAuthDemo.Core.Api;
using OAuthDemo.Core.Api.Response;

namespace OAuthDemo.Api.Controllers
{
    public class ApiBaseController : ApiController
    {
        public ResponseBase Success()
        {
            return new ResponseBase { Code = 0, Message = "操作成功" };
        }

        public ResponseBase<T> Success<T>(T data) where T : class
        {
            return new ResponseBase<T> { Code = 0, Message = "操作成功", Data = data };
        }

        public ResponseBase Error(int code, string message)
        {
            return new ResponseBase { Code = code, Message = message };
        }

        public ResponseBase<T> Error<T>(int code, string message, T data = null) where T : class
        {
            return new ResponseBase<T> { Code = code, Message = message, Data = data };
        }

        public ResponsePaged<T> Success<T>(T data, int totalCount, int pageIndex = 0, int pageSize = 20) where T : class
        {
            return new ResponsePaged<T> { Code = 0, Message = "操作成功", Data = data, TotalCount = totalCount, PageIndex = pageIndex, PageSize = pageSize };
        }

        public UserInfo CurrentUser
        {
            get
            {
                var pricipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                var employeeIdClaim = pricipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var employeeId = employeeIdClaim?.Value;
                if (employeeId != null)
                {
                    var userInfo = new UserInfo { Id = new Guid(employeeId) };
                    return userInfo;
                }
                return null;
            }
        }
    }
}