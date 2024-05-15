using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.QueryObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NuClear.Dapper
{
    public interface IRepository<TKey, TEntity> : IDisposable
        where TEntity : IEntity<TKey>
    {
        TEntity FirstOrDefault(TKey id);

        TEntity FirstOrDefault(Query query, Sort sort = null);

        int Count(string sql, object parameters = null, bool recompile = false);

        /// <summary>
        /// TEntity count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        int Count(Query query);
        bool Exists(TKey id);
        bool Exists(Query query);
        IEnumerable<TEntity> Query(Query query, Sort sort = null);
        IEnumerable<TEntity> Query(string query, object parameters = null);
        IEnumerable<TEntity> QueryPaged(Query query, Pager pager, Sort sort);
        IEnumerable<TEntity> QueryPaged(Query query, Pager pager, Sort sort, AggrQuery aggrQuery);
        IEnumerable<TEntity> QueryPaged(string countSql, object countParameters, string query, object parameters, out int totalCount);
        IEnumerable<TAny> Query<TAny>(string selectSql, Query query, Sort sort = null, bool recompile = false);
        IEnumerable<TAny> Query<TAny>(string selectSql, Query query, string andWhereSql, Dictionary<string, object> parameters, Sort sort = null);
        IEnumerable<TAny> QueryPaged<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null, bool recompile = false);
        IEnumerable<TAny> QueryPaged<TAny>(string selectSql, Query query, Pager pager, Sort sort, string andWhereSql, Dictionary<string, object> parameters, AggrQuery aggrQuery = null);
        IEnumerable<TAny> Query<TAny>(string query, object parameters = null, bool recompile = false);
        IEnumerable<TAny> QueryPaged<TAny>(string countSql, object countParameters, string query, object parameters, out int totalCount) where TAny : class;

        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string split = null) where TReturn : class;
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string split = null) where TReturn : class;

        void Insert(TEntity entity);
        void InsertBatch(IEnumerable<TEntity> entities);
        int Execute(string sql, object parameters = null, CommandType? commandType = null);
        void Delete(TKey id);
        void Delete(TEntity entity);
        void DeleteBatch(params TKey[] ids);
        void DeleteBatch(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateBatch(IEnumerable<TEntity> entities);

        Task<TEntity> FirstOrDefaultAsync(TKey id);

        Task<TEntity> FirstOrDefaultAsync(Query query, Sort sort = null);

        Task<int> CountAsync(string sql, object parameters = null, bool recompile = false);

        /// <summary>
        /// TEntity count
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<int> CountAsync(Query query);
        Task<bool> ExistsAsync(TKey id);
        Task<bool> ExistsAsync(Query query);
        Task<IEnumerable<TEntity>> QueryAsync(Query query, Sort sort = null);
        Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null);
        Task<IEnumerable<TEntity>> QueryPagedAsync(Query query, Pager pager, Sort sort);
        Task<IEnumerable<TEntity>> QueryPagedAsync(Query query, Pager pager, Sort sort, AggrQuery aggrQuery);
        Task<(IEnumerable<TEntity>, int)> QueryPagedAsync(string countSql, object countParameters, string query, object parameters);
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string selectSql, Query query, Sort sort = null, bool recompile = false);
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string selectSql, Query query, string andWhereSql, Dictionary<string, object> parameters, Sort sort = null);
        Task<IEnumerable<TAny>> QueryPagedAsync<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null, bool recompile = false);
        Task<IEnumerable<TAny>> QueryPagedAsync<TAny>(string selectSql, Query query, Pager pager, Sort sort, string andWhereSql, Dictionary<string, object> parameters, AggrQuery aggrQuery = null, bool recompile = false);
        Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null, bool recompile = false);
        Task<(IEnumerable<TAny>, int)> QueryPagedAsync<TAny>(string countSql, object countParameters, string query, object parameters) where TAny : class;
        Task InsertAsync(TEntity entity);
        Task<int> ExecuteAsync(string sql, object parameters = null, CommandType? commandType = null);
        Task DeleteAsync(TKey id);
        Task DeleteAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);




        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string split = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string split = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string split = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string split = null);
    }
}
