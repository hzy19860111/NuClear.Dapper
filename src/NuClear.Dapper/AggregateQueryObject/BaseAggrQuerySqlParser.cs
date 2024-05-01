using NuClear.Dapper.Exceptions;
using System.Linq;

namespace NuClear.Dapper.AggregateQueryObject
{
    public abstract class BaseAggrQuerySqlParser : IAggrQuerySqlParser
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

        protected abstract string ToSql(AggrCriterion aggrCriterion);
    }
}
