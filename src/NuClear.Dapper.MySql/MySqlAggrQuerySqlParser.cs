using NuClear.Dapper.AggregateQueryObject;

namespace NuClear.Dapper.MySql
{
    public class MySqlAggrQuerySqlParser : BaseAggrQuerySqlParser, IAggrQuerySqlParser
    {
        protected override string ToSql(AggrCriterion aggrCriterion)
        {
            string oper = aggrCriterion.Operator.ToSql();
            if (oper == null) return null;

            var aliasName = string.IsNullOrWhiteSpace(aggrCriterion.TableAliasName) ? "" : aggrCriterion.TableAliasName + ".";

            return $"IFNULL({oper}({aliasName}{aggrCriterion.PropertyName}),0) {aggrCriterion.ResultPropertyName}";
        }
    }
}
