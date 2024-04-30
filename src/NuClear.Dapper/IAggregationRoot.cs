namespace NuClear.Dapper
{
    /// <summary>
    /// 聚合根对象
    /// 核心的业务对象都需要从这个对象继承
    /// 整个业务都是围绕聚合根对象来进行的
    /// </summary>
    public interface IAggregationRoot : IEntity
    {
    }

    public interface IAggregationRoot<T> : IEntity<T>
    {
    }
}
