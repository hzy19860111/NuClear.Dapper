namespace NuClear.Dapper.SqlResources.Scripting
{
    public class ExpressionEvaluator
    {
        public bool EvaluateBoolean(string expression, object parameterObject)
        {
            object value = OgnlCache.GetValue(expression, parameterObject);

            if (value == null)
            {
                return false;
            }

            if (value is bool booleanValue)
            {
                return booleanValue;
            }

            if (value is int intValue)
            {
                return intValue != 0;
            }

            return true;
        }
    }
}
