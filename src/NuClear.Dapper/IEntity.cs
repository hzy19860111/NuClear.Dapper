namespace NuClear.Dapper
{
    /// <summary>
    /// 实体，有主键的对象
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        string Id { get; set; }
    }

    public interface IEntity<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        T Id { get; set; }
    }
}