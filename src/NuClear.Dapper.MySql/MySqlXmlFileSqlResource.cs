using System.Collections.Concurrent;
using NuClear.Dapper.Exceptions;
using NuClear.Dapper.SqlResources;
using MySql.Data.MySqlClient;

namespace NuClear.Dapper.MySql
{
    /// <summary>
    /// SqlServer的sql语句xml文件 约定位置为： 引用根目录\SqlResource\SqlServerSqlResource\类型名称.xml
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MySqlXmlFileSqlResource<T> : XmlFileBaseSqlResource<T>
        where T : IEntity
    {
        protected override string Directory => "SqlResource/MySqlSqlResource";
    }
}
