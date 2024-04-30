using System;
using System.Linq;

namespace NuClear.Dapper.SqlServer
{
    internal static class SqlUtility
    {
        private static readonly string[] sqlKeywords = new string[] { "key", "user", "state", "enable" };
        internal static bool IsSqlServerKeyword(string str)
        {
            return sqlKeywords.Any(k => k.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static string HandleSqlServerKeyword(string str)
        {
            if (IsSqlServerKeyword(str)) return "[" + str + "]";
            return str;
        }
    }
}
