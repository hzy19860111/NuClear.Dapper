using System.Collections.Concurrent;
using NuClear.Dapper.Exceptions;
using NuClear.Dapper.SqlResources;

namespace NuClear.Dapper.SqlServer
{
    /// <summary>
    /// SqlServer的sql语句xml文件 约定位置为： 引用根目录\SqlResource\SqlServerSqlResource\类型名称.xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlServerXmlFileSqlResource<T> : XmlFileBaseSqlResource<T>
        where T : IEntity
    {
        private static readonly ConcurrentDictionary<string, bool> lockCheckCache = new ConcurrentDictionary<string, bool>();

        private SqlRegexs SqlRegex => SqlRegexs.Instance;

        protected override string Directory => "SqlResource/SqlServerSqlResource";

        public override string GetSql(string key, object parameterObject)
        {
            string cacheKey = FileName + "_" + key;
            if (lockCheckCache.ContainsKey(cacheKey))
            {
                if (lockCheckCache.TryGetValue(cacheKey, out bool value) && !value)
                {
                    throw new SqlParseException("select 语句必须增加 with(nolock)!");
                }
                return base.GetSql(key, parameterObject);
            }

            string sql = base.GetSql(key, parameterObject);
            if (SqlRegex.SelectRegex.IsMatch(sql) && !SqlRegex.WithNolockRegex.IsMatch(sql))
            {
                lockCheckCache.TryAdd(cacheKey, false);
                throw new SqlParseException("select 语句必须增加 with(nolock)!");
            }
            lockCheckCache.TryAdd(cacheKey, true);
            return sql;
        }
    }
}
