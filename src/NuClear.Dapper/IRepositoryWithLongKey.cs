namespace NuClear.Dapper
{
    public interface IRepositoryWithLongKey<TEntity> : IRepository<long, TEntity>
        where TEntity : IEntity<long>
    {
    }
}
