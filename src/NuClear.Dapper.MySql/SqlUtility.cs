using System;
using System.Linq;

namespace NuClear.Dapper.MySql
{
    internal static class SqlUtility
    {
        private static readonly string[] sqlKeywords = new string[] { "insert", "delete", "update", "select", "distinct", "between",
            "like", "limit", "count", "sum", "max", "min", "avg", "having", "index","key" };
        internal static bool IsSqlServerKeyword(string str)
        {
            return sqlKeywords.Any(k => k.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static string HandleMySqlKeyword(string str)
        {
            if (IsSqlServerKeyword(str)) return "`" + str + "`";
            return str;
        }
    }
}
