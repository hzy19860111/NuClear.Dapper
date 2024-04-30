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

        public virtual int TotalCount { get; set; }
        public virtual int PageCount
        {
            get { if (this.PageSize >= 0) { return (this.TotalCount - 1) / this.PageSize + 1; } return 1; }
        }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public bool IsGetTotalCount { get; set; }

    }
}
