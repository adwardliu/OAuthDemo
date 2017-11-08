namespace OAuthDemo.Core.Api.Response
{
    public class ResponsePaged<T> : ResponseBase<T> where T : class
    {
        /// <summary>
        /// 当前页 索引从0开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        private int _pageCount = 0;

        /// <summary>
        /// 总页数 内部计算即可 不用赋值
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize < 1)
                {
                    return _pageCount;
                }
                _pageCount = TotalCount / PageSize;
                if (TotalCount % PageSize != 0)
                {
                    _pageCount += 1;
                }
                return _pageCount;
            }
            set
            {
                _pageCount = value;
            }
        }
    }
}