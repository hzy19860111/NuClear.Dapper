using System.Collections.Generic;

namespace NuClear.Dapper.QueryObject
{
    public static class SqlParamExtensions
    {
        public static object ToDynamicObject(this List<SqlParam> parameters)
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            var dic = obj as IDictionary<string, object>;
            foreach (var item in parameters)
            {
                dic.Add(item.Name, item.Value);
            }
            return obj;
        }
    }
}
