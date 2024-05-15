using Dapper;
using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.Context;
using NuClear.Dapper.QueryObject;
using NuClear.Dapper.SqlResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace NuClear.Dapper
{
    public abstract class RepositoryBase<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        protected const int Default_CommandTimeout = 360;
        protected abstract IContext Context { get; }
        protected abstract IAggrQuerySqlParser AggrQuerySqlParser { get; }
        protected abstract ISqlParser SqlParser { get; }
        protected abstract ICriterionSqlParser CriterionSqlParser { get; }
        protected abstract ISqlResource<TEntity> SqlResource { get; }

        protected IDbConnection Conn { get { return Context.Connection; } }
        protected IDbTransaction Tran { get { return Context.Transaction; } }

        #region Internal

        protected IEnumerable<dynamic> InternalQuery(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.Query(sql, param, Tran, commandTimeout: commandTimeout ?? Default_CommandTimeout, commandType: commandType);
        }

        protected IEnumerable<T> InternalQuery<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.Query<T>(sql, param, Tran, commandTimeout: commandTimeout ?? Default_CommandTimeout, commandType: commandType);
        }
        public IEnumerable<TReturn> InternalQuery<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = null)
        {
            return Conn.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }
        public IEnumerable<TReturn> InternalQuery<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = null)
        {
            return Conn.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }

        protected int InternalExecute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.Execute(sql, param, Tran, commandTimeout: commandTimeout ?? Default_CommandTimeout, commandType: commandType);
        }

        protected async Task<int> InternalExecuteAsync(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Conn.ExecuteAsync(sql, param, Tran, commandTimeout ?? Default_CommandTimeout, commandType: commandType);
        }

        protected T InternalExecuteScalar<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Conn.ExecuteScalar<T>(sql, param, Tran, commandTimeout ?? Default_CommandTimeout, commandType);
        }

        protected async Task<T> InternalExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Conn.ExecuteScalarAsync<T>(sql, param, Tran, commandTimeout ?? Default_CommandTimeout, commandType);
        }

        protected async Task<IEnumerable<T>> InternalQueryAsync<T>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await Conn.QueryAsync<T>(sql, param, Tran, commandTimeout ?? Default_CommandTimeout, commandType: commandType);
        }

        protected async Task<IEnumerable<TReturn>> InternalQueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = null)
        {
            return await Conn.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }
        protected async Task<IEnumerable<TReturn>> InternalQueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = null)
        {
            return await Conn.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }
        protected async Task<IEnumerable<TReturn>> InternalQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string splitOn = null)
        {
            return await Conn.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }
        protected async Task<IEnumerable<TReturn>> InternalQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string splitOn = null)
        {
            return await Conn.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, transaction: Tran, splitOn: splitOn, commandTimeout: Default_CommandTimeout);
        }

        #endregion

        #region FirstOrDefault

        public virtual TEntity FirstOrDefault(TKey id)
        {
            var query = QueryObject.Query.Create()
                  .Add(Criterion.Create("Id", id, CriteriaOperator.Equal, this.TableAliasName));

            if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                query.Add(Criterion.Create("IsDeleted", false, CriteriaOperator.Equal, this.TableAliasName));
            }
            return FirstOrDefault(query);
        }

        public virtual TEntity FirstOrDefault(Query query, Sort sort = null)
        {
            var whereSql = GetWhereSql(query, out object param);
            var sql = this.SqlParser.CreateTop1SelectSql(SelectSql, whereSql, sort?.Translate());
            return this.Query<TEntity>(sql, param).FirstOrDefault();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TKey id)
        {
            var query = QueryObject.Query.Create()
                  .Add(Criterion.Create("Id", id, CriteriaOperator.Equal, this.TableAliasName));

            if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                query.Add(Criterion.Create("IsDeleted", false, CriteriaOperator.Equal, this.TableAliasName));
            }
            return await FirstOrDefaultAsync(query);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Query query, Sort sort = null)
        {
            var whereSql = GetWhereSql(query, out object param);
            var sortSql = sort?.Translate();

            var top1Sql = this.SqlParser.CreateTop1SelectSql(SelectSql, whereSql, sortSql);

            return await this.InternalQueryFirstOrDefaultAsync<TEntity>(top1Sql, param);
        }

        private async Task<TAny> InternalQueryFirstOrDefaultAsync<TAny>(string query, object parameters = null)
        {
            return await Conn.QueryFirstOrDefaultAsync<TAny>(query, parameters, Tran, commandTimeout: Default_CommandTimeout);
        }

        #endregion

        #region Count
        public virtual int Count(string sql, object parameters = null, bool recompile = false)
        {
            return InternalExecuteScalar<int>(sql + GetRecompileSql(recompile), parameters);
        }

        public virtual int Count(Query query)
        {
            string whereSql = GetWhereSql(query, out object param);
            return this.Count(CombineCountSql(this.SelectSql, whereSql), param);
        }

        public virtual async Task<int> CountAsync(string sql, object parameters = null, bool recompile = false)
        {
            return await InternalExecuteScalarAsync<int>(sql + GetRecompileSql(recompile), parameters);
        }

        public virtual async Task<int> CountAsync(Query query)
        {
            string whereSql = GetWhereSql(query, out object param);
            return await this.CountAsync(CombineCountSql(this.SelectSql, whereSql), param);
        }

        #endregion

        #region Exists

        public virtual bool Exists(TKey id)
        {
            var query = QueryObject.Query.Create().Add(Criterion.Create("Id", id, CriteriaOperator.Equal, this.TableAliasName));
            if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                query.Add(Criterion.Create("IsDeleted", false, CriteriaOperator.Equal));
            }
            var whereSql = GetWhereSql(query, out object param);
            var sql = CombineCountSql(SelectSql, whereSql);
            System.Console.WriteLine($"Exists : {sql}");
            return this.Count(sql, param) > 0;
        }

        public virtual bool Exists(Query query)
        {
            return this.Count(query) > 0;
        }

        public virtual async Task<bool> ExistsAsync(TKey id)
        {
            var query = QueryObject.Query.Create().Add(Criterion.Create("Id", id, CriteriaOperator.Equal, this.TableAliasName));
            if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                query.Add(Criterion.Create("IsDeleted", false, CriteriaOperator.Equal));
            }
            var whereSql = GetWhereSql(query, out object param);
            return await this.CountAsync(CombineCountSql(SelectSql, whereSql), param) > 0;
        }

        public virtual async Task<bool> ExistsAsync(Query query)
        {
            return await this.CountAsync(query) > 0;
        }
        #endregion

        #region Query TEntity
        public virtual IEnumerable<TEntity> Query(string query, object parameters = null)
        {
            return InternalQuery<TEntity>(query, parameters);
        }

        public virtual IEnumerable<TEntity> Query(Query query, Sort sort = null)
        {
            string sql = GetQuerySql(query, sort, out object param);
            return InternalQuery<TEntity>(sql, param);
        }

        public virtual IEnumerable<TEntity> QueryPaged(Query query, Pager pager, Sort sort)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(SelectSql, whereSql), param);
            }
            return InternalQuery<TEntity>(CombinePagedSql(SelectSql, whereSql, sort?.Translate(), pager), param);
        }

        public virtual IEnumerable<TEntity> QueryPaged(Query query, Pager pager, Sort sort, AggrQuery aggrQuery)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(SelectSql, whereSql), param);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(SelectSql, whereSql);
                SetAggr(sql, aggrQuery, param);
            }
            return InternalQuery<TEntity>(CombinePagedSql(SelectSql, whereSql, sort?.Translate(), pager), param);
        }

        public virtual IEnumerable<TEntity> QueryPaged(string countSql, object countParameters, string query, object parameters, out int totalCount)
        {
            totalCount = this.Count(countSql, countParameters);
            return InternalQuery<TEntity>(query, parameters);
        }

        protected string GetQuerySql(Query query, Sort sort, out object param)
        {
            return CombineSql(this.SelectSql, GetWhereSql(query, out param), sort?.Translate());
        }

        protected string GetWhereSql(Query query, out object param)
        {
            param = null;
            if (query == null) return null;
            var result = query.Translate(CriterionSqlParser);
            if (result == null) return null;
            param = result.Parameter;
            return " where " + result.Sql;
        }

        protected string CombineSql(string selectSql, string whereSql, string orderBy = null)
        {
            string sql = selectSql;
            if (!string.IsNullOrWhiteSpace(whereSql))
                sql += whereSql;
            if (!string.IsNullOrWhiteSpace(orderBy))
                sql += orderBy;
            return sql;
        }

        protected string CombineCountSql(string selectSql, string whereSql)
        {
            return string.Format("{0} {1}", this.SqlParser.CreateCountSqlBySelectSql(selectSql), whereSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="sortSql"></param>
        /// <param name="recompile">是否配置OPTION(RECOMPILE)</param>
        /// <returns></returns>
        protected string CombinePagedSql(string selectSql, string whereSql, string sortSql, Pager pager = null)
        {
            return SqlParser.CreatePagedSql(selectSql, whereSql, sortSql, pager);
        }

        protected object AddPagedParams(object param, Pager pager)
        {
            if (param == null)
            {
                IDictionary<string, object> d = new Dictionary<string, object>
                {
                    { "PageIndex", pager.PageIndex },
                    { "PageSize", pager.PageSize }
                };
                return d;
            }
            if (param is IDictionary<string, object> dic)
            {
                dic.Add("PageIndex", pager.PageIndex);
                dic.Add("PageSize", pager.PageSize);
            }
            return param;
        }

        protected string GetAggrSelectSql(string selectSql, AggrQuery query)
        {
            return this.SqlParser.ReplaceFieldSql(selectSql, this.AggrQuerySqlParser.Translate(query));
        }

        protected void SetAggr(string sql, AggrQuery aggrQuery, object param, bool recompile = false)
        {
            aggrQuery.SetResult(InternalQuery(GetAggrSelectSql(sql, aggrQuery) + GetRecompileSql(recompile), param).FirstOrDefault());
        }

        protected string GetRecompileSql(bool recompile)
        {
            return SqlParser.GetRecompileSql(recompile);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Query query, Sort sort = null)
        {
            string sql = GetQuerySql(query, sort, out object param);
            return await InternalQueryAsync<TEntity>(sql, param);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null)
        {
            return await InternalQueryAsync<TEntity>(query, parameters);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPagedAsync(Query query, Pager pager, Sort sort)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(SelectSql, whereSql), param);
            }
            return await InternalQueryAsync<TEntity>(CombinePagedSql(SelectSql, whereSql, sort?.Translate(), pager), param);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryPagedAsync(Query query, Pager pager, Sort sort, AggrQuery aggrQuery)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(SelectSql, whereSql), param);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(SelectSql, whereSql);
                SetAggr(sql, aggrQuery, param);
            }
            return await InternalQueryAsync<TEntity>(CombinePagedSql(SelectSql, whereSql, sort?.Translate(), pager), param);
        }

        public virtual async Task<(IEnumerable<TEntity>, int)> QueryPagedAsync(string countSql, object countParameters, string query, object parameters)
        {
            var totalCount = await this.CountAsync(countSql, countParameters);
            var result = await InternalQueryAsync<TEntity>(query, parameters);

            return (result, totalCount);
        }

        #endregion

        #region Query TAny
        public virtual IEnumerable<TAny> Query<TAny>(string selectSql, Query query, Sort sort = null, bool recompile = false)
        {
            string whereSql = GetWhereSql(query, out object param);
            return InternalQuery<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()) + GetRecompileSql(recompile), param);
        }
        public virtual IEnumerable<TAny> QueryPaged<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null, bool recompile = false)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(selectSql, whereSql), param, recompile);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggr(sql, aggrQuery, param, recompile);
            }
            return InternalQuery<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager) + GetRecompileSql(recompile), param);
        }

        public virtual IEnumerable<TAny> Query<TAny>(string query, object parameters = null, bool recompile = false)
        {
            return InternalQuery<TAny>(query + GetRecompileSql(recompile), parameters);
        }

        public virtual IEnumerable<TAny> QueryPaged<TAny>(string countSql, object countParameters, string query, object parameters, out int totalCount) where TAny : class
        {
            totalCount = this.Count(countSql, parameters);
            return InternalQuery<TAny>(query, parameters);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string split = null) where TReturn : class
        {
            return InternalQuery<TFirst, TSecond, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string split = null) where TReturn : class
        {
            return InternalQuery<TFirst, TSecond, TThird, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }

        public virtual IEnumerable<TAny> QueryPaged<TAny>(string selectSql, Query query, Pager pager, Sort sort, string andWhereSql, Dictionary<string, object> parameters, AggrQuery aggrQuery = null)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            whereSql = AddWhereSql(whereSql, andWhereSql);

            param = AddPagedParams(param, pager);
            IDictionary<string, object> dic = AddParams(param, parameters);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(selectSql, whereSql), dic);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggr(sql, aggrQuery, param);
            }
            return InternalQuery<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager), dic);
        }

        private static IDictionary<string, object> AddParams(object sourceParam, Dictionary<string, object> addParameters)
        {
            IDictionary<string, object> dic = sourceParam as IDictionary<string, object>;
            if (dic != null && addParameters != null && addParameters.Any())
            {
                foreach (var item in addParameters)
                {
                    dic.Add(item);
                }
            }

            return dic;
        }

        public virtual IEnumerable<TAny> Query<TAny>(string selectSql, Query query, string andWhereSql, Dictionary<string, object> parameters, Sort sort = null)
        {
            string whereSql = GetWhereSql(query, out object param);
            whereSql = AddWhereSql(whereSql, andWhereSql);

            IDictionary<string, object> dic = AddParams(param, parameters);
            return InternalQuery<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()), dic);
        }

        private string AddWhereSql(string whereSql, string andWhereSql)
        {
            if (string.IsNullOrWhiteSpace(whereSql))
            {
                whereSql = " where 1=1 " + andWhereSql;
            }
            else
            {
                whereSql += " " + andWhereSql;
            }

            return whereSql;
        }


        public virtual async Task<IEnumerable<TAny>> QueryAsync<TAny>(string selectSql, Query query, Sort sort = null, bool recompile = false)
        {
            string whereSql = GetWhereSql(query, out object param);
            return await InternalQueryAsync<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()) + GetRecompileSql(recompile), param);
        }

        public virtual async Task<IEnumerable<TAny>> QueryAsync<TAny>(string selectSql, Query query, string andWhereSql, Dictionary<string, object> parameters, Sort sort = null)
        {
            string whereSql = GetWhereSql(query, out object param);
            whereSql = AddWhereSql(whereSql, andWhereSql);

            IDictionary<string, object> dic = AddParams(param, parameters);
            return await InternalQueryAsync<TAny>(CombineSql(selectSql, whereSql, sort?.Translate()), dic);
        }

        public virtual async Task<IEnumerable<TAny>> QueryPagedAsync<TAny>(string selectSql, Query query, Pager pager, Sort sort, AggrQuery aggrQuery = null, bool recompile = false)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            param = AddPagedParams(param, pager);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(selectSql, whereSql), param, recompile);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggr(sql, aggrQuery, param, recompile);
            }
            return await InternalQueryAsync<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager) + GetRecompileSql(recompile), param);
        }

        public virtual async Task<IEnumerable<TAny>> QueryPagedAsync<TAny>(string selectSql, Query query, Pager pager, Sort sort, string andWhereSql, Dictionary<string, object> parameters, AggrQuery aggrQuery = null, bool recompile = false)
        {
            if (pager == null)
                throw new ArgumentNullException(nameof(pager));

            string whereSql = GetWhereSql(query, out object param);
            whereSql = AddWhereSql(whereSql, andWhereSql);

            param = AddPagedParams(param, pager);
            IDictionary<string, object> dic = AddParams(param, parameters);
            if (pager.IsGetTotalCount)
            {
                pager.TotalCount = this.Count(CombineCountSql(selectSql, whereSql), dic, recompile);
            }
            if (aggrQuery != null)
            {
                var sql = CombineSql(selectSql, whereSql);
                SetAggr(sql, aggrQuery, param, recompile);
            }
            return await InternalQueryAsync<TAny>(CombinePagedSql(selectSql, whereSql, sort?.Translate(), pager) + GetRecompileSql(recompile), dic);
        }

        public virtual async Task<IEnumerable<TAny>> QueryAsync<TAny>(string query, object parameters = null, bool recompile = false)
        {
            return await InternalQueryAsync<TAny>(query + GetRecompileSql(recompile), parameters);
        }

        public virtual async Task<(IEnumerable<TAny>, int)> QueryPagedAsync<TAny>(string countSql, object countParameters, string query, object parameters) where TAny : class
        {
            var totalCount = await this.CountAsync(countSql, parameters);
            var result = await InternalQueryAsync<TAny>(query, parameters);

            return (result, totalCount);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string split = null)
        {
            return await InternalQueryAsync<TFirst, TSecond, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string split = null)
        {
            return await InternalQueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, string split = null)
        {
            return await InternalQueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }
        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, string split = null)
        {
            return await InternalQueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param, splitOn: split ?? "Id");
        }

        #endregion

        #region Insert

        public virtual void Insert(TEntity entity)
        {
            InternalExecute(InsertSql, entity);
        }

        public virtual void InsertBatch(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Insert(item);
            }
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await InternalExecuteAsync(InsertSql, entity);
        }

        #endregion

        #region Execute
        public virtual int Execute(string sql, object parameters = null, CommandType? commandType = null)
        {
            return InternalExecute(sql, parameters, commandType: commandType);
        }

        public virtual async Task<int> ExecuteAsync(string sql, object parameters = null, CommandType? commandType = null)
        {
            return await InternalExecuteAsync(sql, parameters, commandType: commandType);
        }

        #endregion

        #region Update
        public virtual void Update(TEntity entity)
        {
            InternalExecute(UpdateSql, entity);
        }

        public virtual void UpdateBatch(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Update(entity);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await InternalExecuteAsync(UpdateSql, entity);
        }

        #endregion

        #region Delete
        public virtual void Delete(TEntity entity)
        {
            Execute(DeleteSql, new { entity.Id });
        }
        public virtual void Delete(TKey id)
        {
            Execute(DeleteSql, new { Id = id });
        }
        public virtual void DeleteBatch(params TKey[] ids)
        {
            foreach (var id in ids)
            {
                this.Delete(id);
            }
        }
        public virtual void DeleteBatch(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Delete(entity);
            }
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            await ExecuteAsync(DeleteSql, new { entity.Id });
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            await ExecuteAsync(DeleteSql, new { Id = id });
        }

        #endregion

        #region IDisposed

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                Context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Sql
        public virtual string TableAliasName { get { return ""; } }
        public virtual string SelectSql { get { return SqlResource.Select; } }
        public virtual string InsertSql { get { return SqlResource.Insert; } }
        public virtual string UpdateSql { get { return SqlResource.Update; } }
        public virtual string DeleteSql { get { return SqlResource.Delete; } }
        #endregion
    }

    public abstract class RepositoryBaseWithLongKey<TEntity> : RepositoryBase<long, TEntity>
        where TEntity : IEntityWithLongKey
    {
    }

}
