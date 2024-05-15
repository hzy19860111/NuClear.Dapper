namespace NuClear.Dapper
{
    public class Pager
    {
        public Pager()
        {
            this.TotalCount = 0;
            this.PageSize = 20;
            this.PageIndex = 1;
            this.IsGetTotalCount = true;
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public virtual int PageCount
        {
            get { if (this.PageSize >= 0) { return (this.TotalCount - 1) / this.PageSize + 1; } return 1; }
        }
        /// <summary>
        /// 每页数量
        /// </summary>
        public virtual int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public virtual int PageIndex { get; set; }
        /// <summary>
        /// 是否获取总记录数
        /// </summary>
        public bool IsGetTotalCount { get; set; }

    }
}
