using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuClear.Dapper.SqlServer
{
    public static class IRepositoryExtensions
    {
        private static SqlServerRepositoryBase<TKey, TEntity> CheckAndConvertToSqlServerRepositoryBase<TKey, TEntity>(IRepository<TKey, TEntity> repository)
          where TEntity : IEntity<TKey>
        {
            if (!(repository is SqlServerRepositoryBase<TKey, TEntity> sqlServerRepository))
            {
                throw new NotSupportedException("不支持recompile参数！");
            }

            return sqlServerRepository;
        }

        public static int CountWithRecompile<TKey, TEntity>(this IRepository<TKey, TEntity> repository, string sql, object parameters = null)
            where TEntity : IEntity<TKey>
        {
            var sqlServerRepository = CheckAndConvertToSqlServerRepositoryBase(repository);
            return sqlServerRepository.CountWithRecompile(sql, parameters);
        }



        public static async Task<int> CountWithRecompileAsync<TKey, TEntity>(this IRepository<TKey, TEntity> repository, string sql, object parameters = null)
            where TEntity : IEntity<TKey>
        {
            var sqlServerRepository = CheckAndConvertToSqlServerRepositoryBase(repository);
            return await sqlServerRepository.CountWithRecompileAsync(sql, parameters);
        }


        public static IEnumerable<TAny> QueryWithRecompile<TKey, TEntity, TAny>(this IRepository<TKey, TEntity> repository, string selectSql, Query query, Sort sort = null)
                 where TEntity : IEntity<TKey>
        {
            var sqlServerRepository = CheckAndConvertToSqlServerRepositoryBase(repository);
            return sqlServerRepository.QueryWithRecompile<TKey, TEntity, TAny>(selectSql, query, sort);
        }
        public static IEnumerable<TAny> QueryWithRecompilePaged<TKey, TEntity, TAny>(this IRepository<TKey, TEntity> repository, string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null)
                 where TEntity : IEntity<TKey>
        {
            var sqlServerRepository = CheckAndConvertToSqlServerRepositoryBase(repository);
            return sqlServerRepository.QueryWithRecompilePaged<TKey, TEntity, TAny>(selectSql, query, pager, sort, aggrQuery);
        }

        public static IEnumerable<TAny> QueryWithRecompile<TKey, TEntity, TAny>(this IRepository<TKey, TEntity> repository, string query, object parameters = null)
                 where TEntity : IEntity<TKey>
        {
            var sqlServerRepository = CheckAndConvertToSqlServerRepositoryBase(repository);
            return sqlServerRepository.QueryWithRecompile<TKey, TEntity, TAny>(query, parameters);
        }

    }
}
