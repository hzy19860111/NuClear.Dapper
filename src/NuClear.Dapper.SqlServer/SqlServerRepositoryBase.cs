using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.SqlServer
{
    public abstract class SqlServerRepositoryBase<TKey, TEntity> : RepositoryBase<TKey, TEntity>
         where TEntity : IEntity<TKey>
    {
        protected override IAggrQuerySqlParser AggrQuerySqlParser => new SqlServerAggrQuerySqlParser();
        protected override ICriterionSqlParser CriterionSqlParser => new SqlServerCriterionSqlParser();
        protected override ISqlParser SqlParser => new SqlServerSqlParser();
    }
    public abstract class SqlServerRepositoryBaseWithLongKey<TEntity> : SqlServerRepositoryBase<long, TEntity>
        where TEntity : IEntityWithLongKey
    {
    }
}
