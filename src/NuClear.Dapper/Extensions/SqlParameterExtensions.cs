using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace NuClear.Dapper.Extensions
{
    public static class SqlParameterExtensions
    {
        public static object ToDynamicObject(this List<SqlParameter> parameters)
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            var dic = obj as IDictionary<string, object>;
            foreach (var item in parameters)
            {
                dic.Add(item.ParameterName, item.Value);
            }
            return obj;
        }
    }
}
