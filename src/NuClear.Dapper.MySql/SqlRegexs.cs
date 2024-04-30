using System;
using System.Text.RegularExpressions;

namespace NuClear.Dapper.MySql
{
    /// <summary>
    /// 正则表达式类
    /// </summary>
    internal class SqlRegexs 
    {
        private SqlRegexs() { }

        private static readonly Lazy<SqlRegexs> LazyRegexs = new Lazy<SqlRegexs>(() => new SqlRegexs());

        public static SqlRegexs Instance { get { return LazyRegexs.Value; } }

        public Regex SelectRegex => new Regex("select", RegexOptions.IgnoreCase);

        public Regex WhereRegex => new Regex(@"\s*where\s*", RegexOptions.IgnoreCase);

        public Regex OrderByRegex => new Regex(@"(.*)?order\s*by.*", RegexOptions.IgnoreCase);

        public Regex FieldListRegex => new Regex(@"select([\s\S]*?)\s+from\s+", RegexOptions.IgnoreCase);
    }
}
