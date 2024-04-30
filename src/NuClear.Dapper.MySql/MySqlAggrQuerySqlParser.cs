using System.Linq;
using NuClear.Dapper.AggregateQueryObject;
using NuClear.Dapper.Exceptions;

namespace NuClear.Dapper.MySql
{
    public class MySqlAggrQuerySqlParser : IAggrQuerySqlParser
    {
        public string Translate(AggrQuery query)
        {
            if (query.Criterions.Any())
            {
                int i = 0;
                foreach (var criterion in query.Criterions)
                {
                    criterion.ResultPropertyName = "Result" + i.ToString();
                    i++;
                }
                return " " + string.Join(",", query.Criterions.Select(c => ToSql(c)).ToArray()) + " ";
            }
            throw new SqlParseException("未指定聚合函数！");
        }

        private string ToSql(AggrCriterion aggrCriterion)
        {
            string oper = ToSql(aggrCriterion.Operator);
            if (oper == null) return null;

            var aliasName = string.IsNullOrWhiteSpace(aggrCriterion.TableAliasName) ? "" : aggrCriterion.TableAliasName + ".";

            return string.Format("IFNULL({0}({1}{2}),0) {3}", oper, aliasName, aggrCriterion.PropertyName, aggrCriterion.ResultPropertyName);
        }

        private string ToSql(AggrCriteriaOperator co)
        {
            switch (co)
            {
                case AggrCriteriaOperator.Sum:
                    return "SUM";
                case AggrCriteriaOperator.Count:
                    return "COUNT";
                case AggrCriteriaOperator.Max:
                    return "MAX";
                case AggrCriteriaOperator.Min:
                    return "MIN";
                case AggrCriteriaOperator.Avg:
                    return "AVG";
                default:
                    return null;
            }
        }
    }
}
