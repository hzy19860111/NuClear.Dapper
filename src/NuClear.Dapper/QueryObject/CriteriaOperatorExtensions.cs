namespace NuClear.Dapper.QueryObject
{
    public static class CriteriaOperatorExtensions
    {
        public static string ToSqlOperator(this CriteriaOperator criteriaOperator)
        {
            switch (criteriaOperator)
            {
                case CriteriaOperator.Equal:
                    return "=";
                case CriteriaOperator.GreaterThan:
                    return ">";
                case CriteriaOperator.GreaterThanOrEqual:
                    return ">=";
                case CriteriaOperator.LessThen:
                    return "<";
                case CriteriaOperator.LessThenOrEqual:
                    return "<=";
                case CriteriaOperator.Like:
                case CriteriaOperator.LeftLike:
                case CriteriaOperator.RightLike:
                    return "like";
                case CriteriaOperator.NotLike:
                    return "not like";
                case CriteriaOperator.In:
                    return "in";
                case CriteriaOperator.NotIn:
                    return "not in";
                case CriteriaOperator.NotEqual:
                    return "<>";
                case CriteriaOperator.IsNull:
                    return "IS NULL";
                case CriteriaOperator.IsNotNull:
                    return "IS NOT NULL";
                default:
                    return null;
            }
        }
    }
}
