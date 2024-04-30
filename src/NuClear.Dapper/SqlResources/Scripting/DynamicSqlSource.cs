using System.Collections.Generic;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class DynamicSqlSource : ISqlSource
    {
        private readonly Dictionary<string, MixedSqlNode> _sqlSynamicContexts;

        public DynamicSqlSource(Dictionary<string, MixedSqlNode> sqlNodes)
        {
            _sqlSynamicContexts = sqlNodes;
        }

        public string GetSql(string key, object parameterObject = null)
        {
            if (_sqlSynamicContexts.TryGetValue(key, out var sqlNode))
            {
                DynamicContext context = new DynamicContext(parameterObject);
                sqlNode.Apply(context);

                return context.GetSql();
            }

            throw new KeyNotFoundException($"key:{key} 未查找到Sql！");
        }
    }
}
