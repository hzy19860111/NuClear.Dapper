namespace NuClear.Dapper
{
    public class SortCondition
    {
        public string SortKey { get; private set; }
        public bool IsAscending { get; private set; }
        public string TableAliasName { get; private set; }
        private SortCondition(string orderKey, bool isAscending, string tableAliasName)
        {
            this.SortKey = orderKey;
            this.IsAscending = isAscending;
            this.TableAliasName = tableAliasName;
        }

        public static SortCondition Create(string orderKey, bool isAscending = true, string tableAliasName = null)
        {
            return new SortCondition(orderKey, isAscending, tableAliasName);
        }
    }
    public static class SortConditionExtensions
    {
        public static string ToSql(this SortCondition sort)
        {
            var aliasName = string.IsNullOrWhiteSpace(sort.TableAliasName) ? "" : sort.TableAliasName + ".";
            return string.Format("{0}{1} {2}", aliasName, sort.SortKey, sort.IsAscending ? "asc" : "desc");
        }
    }
}
