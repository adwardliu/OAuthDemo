using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OAuthDemo.Core.Api;
using OAuthDemo.Core.Api.Response;

namespace OAuthDemo.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UserController : ApiBaseController
    {
        [Route("Info")]
        public ResponseBase<UserInfo> GetCurrentUser()
        {
            try
            {
                return Success<UserInfo>(CurrentUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        [AllowAnonymous]
        [Route("List")]
        public ResponsePaged<List<UserInfo>> GetUserList()
        {
            try
            {
                var userList = new List<UserInfo>();
                for (int i = 0; i < 20; i++)
                {
                    userList.Add(new UserInfo { Id = new Guid() });
                }
                return Success<List<UserInfo>>(userList, 100);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
