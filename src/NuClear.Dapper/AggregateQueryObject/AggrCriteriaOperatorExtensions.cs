namespace NuClear.Dapper.AggregateQueryObject
{
    public static class AggrCriteriaOperatorExtensions
    {
        public static string ToSql(this AggrCriteriaOperator aggrCriteriaOperator)
        {
            switch (aggrCriteriaOperator)
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
