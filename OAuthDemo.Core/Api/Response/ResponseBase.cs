using Newtonsoft.Json;

namespace OAuthDemo.Core.Api.Response
{
    /// <summary>
    /// 响应实体类的基类
    /// </summary>
    public class ResponseBase
    {
        /// <summary>
        /// 业务操作结果代码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 业务操作结果消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ResponseBase<T> : ResponseBase where T : class
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}