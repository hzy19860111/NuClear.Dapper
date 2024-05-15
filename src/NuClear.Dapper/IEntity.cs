namespace NuClear.Dapper
{
    /// <summary>
    /// 实体，有主键的对象
    /// </summary>
    public interface IEntity
    {
    }

    public interface IEntityWithLongKey : IEntity<long>
    {

    }

    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Id { get; set; }
    }
}