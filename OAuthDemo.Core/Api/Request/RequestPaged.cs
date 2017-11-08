namespace OAuthDemo.Core.Api.Request
{
    /// <summary>
    /// 分页数据请求类
    /// </summary>
    public class RequestPaged : RequestBase
    {
        public RequestPaged()
        {
            PageSize = 20;
            PageIndex = 0;
        }

        /// <summary>
        /// 每页最多记录数
        /// </summary>        
        public int PageSize { get; set; }

        /// <summary>
        /// 页索引 从0开始
        /// </summary>        
        public int PageIndex { get; set; }
    }
}