using ognl;
using System.Text;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class DynamicContext
    {
        static DynamicContext()
        {
            OgnlRuntime.setPropertyAccessor(typeof(object), new ContextAccessor());
        }

        private readonly StringBuilder _stringBuilder = new StringBuilder(" ");

        private readonly object _parameterObject;

        public object ParameterObject
        {
            get { return this._parameterObject; }
        }

        public DynamicContext(object parameterObject)
        {
            _parameterObject = parameterObject;
        }

        public void AppendSql(string sql)
        {
            _stringBuilder.Append(sql);
        }

        public string GetSql()
        {
            return _stringBuilder.ToString().Trim();
        }
    }
}
