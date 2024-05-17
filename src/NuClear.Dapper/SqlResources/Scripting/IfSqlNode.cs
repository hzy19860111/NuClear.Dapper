namespace NuClear.Dapper.SqlResources.Scripting
{
    public class IfSqlNode : ISqlNode
    {
        private readonly ExpressionEvaluator _evaluator;
        private readonly string _test;
        private readonly ISqlNode _contents;

        public IfSqlNode(ISqlNode contents, string test)
        {
            this._test = test;
            this._contents = contents;
            this._evaluator = new ExpressionEvaluator();
        }

        public bool Apply(DynamicContext context)
        {
            if (_evaluator.EvaluateBoolean(_test, context.ParameterObject))
            {
                _contents.Apply(context);
                return true;
            }
            return false;
        }
    }
}
