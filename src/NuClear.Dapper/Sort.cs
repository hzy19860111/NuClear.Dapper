using System.Collections.Generic;
using System.Linq;

namespace NuClear.Dapper
{
    public class Sort
    {
        public static Sort DefaultSort { get { return new Sort().Add(SortCondition.Create("CreateTime", false)); } }
        private Sort()
        {
        }

        private readonly List<SortCondition> _sort = new List<SortCondition>();
        public IEnumerable<SortCondition> Sorts { get { return this._sort; } }

        public Sort Add(SortCondition sortCondition)
        {
            _sort.Add(sortCondition);
            return this;
        }

        public static Sort Create()
        {
            return new Sort();
        }

        public static Sort CreateCreateTimeAscSort(string tableAliasName = "")
        {
            return new Sort().Add(SortCondition.Create("CreateTime", true, tableAliasName));
        }

        public static Sort CreateCreateTimeDescSort(string tableName = "")
        {
            return new Sort().Add(SortCondition.Create("CreateTime", false, tableName));
        }

        public static Sort Create(string orderKey, bool isAscending = true, string tableAliasName = null)
        {
            return new Sort().Add(SortCondition.Create(orderKey, isAscending, tableAliasName));
        }
    }

    public static class SortExtensions
    {
        public static string Translate(this Sort query)
        {
            string[] tempSQLs = query.Sorts.Select(s => s.ToSql()).ToArray();

            if (tempSQLs.Length == 0)
                return null;

            return string.Format(" order by {0}", string.Join(",", tempSQLs));
        }
    }
}
