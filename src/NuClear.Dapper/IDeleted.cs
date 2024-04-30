namespace NuClear.Dapper
{
    /// <summary>
    /// 逻辑删除标识
    /// </summary>
    public interface IDeleted
    {
        bool IsDeleted { get; set; }
    }
}
