using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.SqlServer
{
    public abstract class SqlServerRepositoryBase<T> : RepositoryBase<T>
         where T : IEntity
    {
        protected override IAggrQuerySqlParser AggrQuerySqlParser => new SqlServerAggrQuerySqlParser();
        protected override ICriterionSqlParser CriterionSqlParser => new SqlServerCriterionSqlParser();
        protected override ISqlParser SqlParser => new SqlServerSqlParser();
    }
}
