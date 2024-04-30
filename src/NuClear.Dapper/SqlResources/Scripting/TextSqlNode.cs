namespace NuClear.Dapper.SqlResources.Scripting
{
    public class TextSqlNode : ISqlNode
    {
        private readonly string _text;

        public TextSqlNode(string text)
        {
            this._text = text;
        }

        public bool Apply(DynamicContext context)
        {
            context.AppendSql(_text);
            return true;
        }
    }
}
