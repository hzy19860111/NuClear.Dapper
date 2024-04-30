using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NuClear.Dapper.SqlResources.Scripting;

namespace NuClear.Dapper.SqlResources
{
    public static class XMLScriptBuilder
    {
        public static ISqlSource ParseScriptNode(XElement context)
        {
            var sqlNodes = ParseSql(context);
            return new DynamicSqlSource(sqlNodes);
        }


        private static Dictionary<string, MixedSqlNode> ParseSql(XElement root)
        {
            Dictionary<string, MixedSqlNode> sqlNodes = new Dictionary<string, MixedSqlNode>();

            var sqls = root.Elements().ToList();
            foreach (var sql in sqls)
            {
                var key = sql.Name.LocalName;
                sqlNodes.Add(key, sql.ParseDynamicTags());
            }

            return sqlNodes;
        }
    }
}
