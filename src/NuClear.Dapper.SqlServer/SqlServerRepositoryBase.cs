using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace NuClear.Dapper.SqlServer
{
    public abstract class SqlServerRepositoryBase<TKey, TEntity> : RepositoryBase<TKey, TEntity>
         where TEntity : IEntity<TKey>
    {
        protected override IAggrQuerySqlParser AggrQuerySqlParser => new SqlServerAggrQuerySqlParser();
        protected override ICriterionSqlParser CriterionSqlParser => new SqlServerCriterionSqlParser();
        protected override ISqlParser SqlParser => new SqlServerSqlParser();


        protected string GetRecompileSql()
        {
            return " OPTION(RECOMPILE)";
        }

        protected void SetAggrWithRecompile(string sql, AggrQuery aggrQuery, object param)
        {
            aggrQuery.SetResult(InternalQuery(GetAggrSelectSql(sql, aggrQuery) + GetRecompileSql(), param).FirstOrDefault());
        }

        public virtual int CountWithRecompile(string sql, object parameters = null)
        {
            return InternalExecuteScalar<int>(sql + GetRecompileSql(), parameters);
        }

        public virtual async Task<int> CountWithRecompileAsync(string sql, object parameters = null)
        {
            return await InternalExecuteScalarAsync<int>(sql + GetRecompileSql(), parameters);
        }

        public virtual IEnumerable<TAny> QueryWithRecompile<TAny>(string selectSql, Query query, Sort sort = null)
        {
            string whereSql = GetWhereSql(query, out object param);
            return InternalQuery<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()) + GetRecompileSql(), param);
        }
        public virtual IEnumerable<TAny> QueryPagedWithRecompile<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.CountWithRecompile(CombineCountSql(selectSql, whereSql), param);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggrWithRecompile(sql, aggrQuery, param);
            }
            return InternalQuery<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager) + GetRecompileSql(), param);
        }

        public virtual IEnumerable<TAny> QueryWithRecompile<TAny>(string query, object parameters = null)
        {
            return InternalQuery<TAny>(query + GetRecompileSql(), parameters);
        }

        public virtual async Task<IEnumerable<TAny>> QueryWithRecompileAsync<TAny>(string selectSql, Query query, Sort sort = null)
        {
            string whereSql = GetWhereSql(query, out object param);
            return await InternalQueryAsync<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()) + GetRecompileSql(), param);
        }

        public virtual async Task<IEnumerable<TAny>> QueryPagedWithRecompileAsync<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.CountWithRecompile(CombineCountSql(selectSql, whereSql), param);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggrWithRecompile(sql, aggrQuery, param);
            }
            return await InternalQueryAsync<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager) + GetRecompileSql(), param);
        }

        public virtual async Task<IEnumerable<TAny>> QueryWithRecompileAsync<TAny>(string query, object parameters = null)
        {
            return await InternalQueryAsync<TAny>(query + GetRecompileSql(), parameters);
        }
    }
    public abstract class SqlServerRepositoryBaseWithLongKey<TEntity> : SqlServerRepositoryBase<long, TEntity>
        where TEntity : IEntityWithLongKey
    {
    }
}
