using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.MySql
{
    public abstract class MySqlRepositoryBase<T> : RepositoryBase<T>
         where T : IEntity
    {
        protected override IAggrQuerySqlParser AggrQuerySqlParser => new MySqlAggrQuerySqlParser();
        protected override ICriterionSqlParser CriterionSqlParser => new MySqlCriterionSqlParser();
        protected override ISqlParser SqlParser => new MySqlSqlParser();
    }
}
