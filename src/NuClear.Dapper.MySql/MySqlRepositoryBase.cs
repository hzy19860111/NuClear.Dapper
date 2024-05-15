using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.MySql
{
    public abstract class MySqlRepositoryBase<TKey, TEntity> : RepositoryBase<TKey, TEntity>
         where TEntity : IEntity<TKey>
    {
        protected override IAggrQuerySqlParser AggrQuerySqlParser => new MySqlAggrQuerySqlParser();
        protected override ICriterionSqlParser CriterionSqlParser => new MySqlCriterionSqlParser();
        protected override ISqlParser SqlParser => new MySqlSqlParser();
    }
    public abstract class MySqlRepositoryBaseWithLongKey<TEntity> : MySqlRepositoryBase<long, TEntity>
         where TEntity : IEntityWithLongKey
    {
    }
}
