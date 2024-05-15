using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper
{
    /// <summary>
    /// 查询对象 
    /// </summary>
    public interface ISearch
    {
        Query CreateQuery();
    }
}
