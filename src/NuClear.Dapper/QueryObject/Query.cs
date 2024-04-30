using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NuClear.Dapper.QueryObject
{
    public class Query
    {
        private readonly List<Criterion> _criterions = new List<Criterion>();
        public IEnumerable<Criterion> Criterions { get { return _criterions; } }

        private readonly List<Query> subQueries = new List<Query>();
        public IEnumerable<Query> SubQueries { get { return subQueries; } }

        public QueryOperator Operator { get; }

        private Query() : this(QueryOperator.And) { }

        private Query(QueryOperator queryOperator)
        {
            Operator = queryOperator;
        }

        public Query Add(Criterion criterion)
        {
            _criterions.Add(criterion);
            return this;
        }

        public Query AddSubQuery(Query subQuery)
        {
            subQueries.Add(subQuery);
            return this;
        }
        internal void HandleRepeatProperty()
        {
            var criterions = GetAllCriterions();
            var repeatProperties = criterions.Where(x => x.Operator != CriteriaOperator.None).GroupBy(c => c.PropertyName.ToLower())
                .Where(c => c.Count() > 1)
                .Select(c => new { PropertyName = c.Key, Count = c.Count() }).ToArray();

            foreach (var repeatProperty in repeatProperties)
            {
                int index = 0;
                foreach (var criterion in criterions.Where(x => x.Operator != CriteriaOperator.None).Where(c => c.PropertyName
                    .Equals(repeatProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase)).ToArray())
                {
                    criterion.RenamePropertyName(criterion.PropertyName + index.ToString());
                    index++;
                }
            }
        }

        private List<Criterion> GetAllCriterions()
        {
            List<Criterion> result = new List<Criterion>();
            result.AddRange(this._criterions);
            if (this.subQueries.Count > 0)
            {
                result.AddRange(this.subQueries.SelectMany(subQuery => subQuery.GetAllCriterions()));
            }
            return result;
        }

        public static Query Create()
        {
            return new Query();
        }

        public static Query Create(QueryOperator queryOperator)
        {
            return new Query(queryOperator);
        }
    }

    public static class QueryExtensions
    {
        /// <summary>
        /// 将Query解析为TranslateResult
        /// </summary>
        /// <param name="query"></param>
        /// <param name="handleRepeatProperty">是否处理重复属性（默认True，只有Query父节点需要处理）</param>
        /// <returns></returns>
        public static TranslateResult Translate(this Query query, ICriterionSqlParser criterionSqlParser, bool handleRepeatProperty = true)
        {
            if (handleRepeatProperty)
            {
                query.HandleRepeatProperty();
            }
            List<string> tempSQLs = new List<string>();
            TranslateResult result = new TranslateResult();

            if (query.Criterions.Any())
            {
                tempSQLs = query.Criterions.Select(c =>
                {
                    var sql = "";
                    if (c is Criterion.TableValueCriterion)
                        sql = criterionSqlParser.ToTableValueSql(c);
                    else
                        sql = criterionSqlParser.ToSql(c);
                        
                    if (c.Operator != CriteriaOperator.IsNull && c.Operator != CriteriaOperator.IsNotNull
                        && c.Operator != CriteriaOperator.None
                        && !(c is Criterion.TableValueCriterion))
                    {
                        result.SqlParameters.Add(new SqlParameter(c.PropertyParameterName, c.Value));
                    }
                    return sql;
                }).Where(sql => !string.IsNullOrEmpty(sql)).ToList();
            }
            if (query.SubQueries.Any())
            {
                foreach (var subQuery in query.SubQueries)
                {
                    var subResult = subQuery.Translate(criterionSqlParser, handleRepeatProperty: false);
                    if (subResult == null)
                        continue;

                    tempSQLs.Add(string.Format("({0})", subResult.Sql));
                    result.SqlParameters.AddRange(subResult.SqlParameters);
                }
            }
            if (tempSQLs.Count == 0)
                return null;

            result.Sql = string.Join(query.Operator == QueryOperator.And ? " and " : " or ", tempSQLs);
            return result;
        }
    }
}
