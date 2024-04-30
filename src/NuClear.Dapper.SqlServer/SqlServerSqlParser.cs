using NuClear.Dapper.Exceptions;
using System.Text.RegularExpressions;

namespace NuClear.Dapper.SqlServer
{
    public class SqlServerSqlParser : ISqlParser
    {
        public string CreateCountSqlBySelectSql(string selectSql)
        {
            return ReplaceFieldSql(RemoveSortBySql(selectSql), "Count(1)");
        }
        private string RemoveSortBySql(string selectSql)
        {
            var sortBySql = GetSortBySql(selectSql);
            if (string.IsNullOrWhiteSpace(sortBySql))
                return selectSql;
            return selectSql.Replace(sortBySql, "");
        }

        private string GetSortBySql(string sql)
        {
            foreach (Match item in SqlRegexs.Instance.OrderByRegex.Matches(sql))
            {
                return item.Value;
            }
            return null;
        }


        public string CreatePagedSql(string selectSql, string whereSql = null, string sortSql = null, Pager pager = null)
        {
            return string.Format("{0} {1} {2} {3}", selectSql, whereSql, sortSql,
                "OFFSET (@PageIndex-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY");
        }

        public string CreateTop1SelectSql(string selectSql, string whereSql = "", string sortSql = "")
        {
            var fieldListSql = GetFieldListSql(selectSql);
            var sql = ReplaceFieldSql(selectSql, " TOP 1 " + fieldListSql);
            if (string.IsNullOrWhiteSpace(whereSql)) return sql + sortSql;

            return sql + (SqlRegexs.Instance.WhereRegex.IsMatch(whereSql) ? whereSql : " where " + whereSql) + sortSql;
        }

        private string GetFieldListSql(string sql)
        {
            foreach (Match item in SqlRegexs.Instance.FieldListRegex.Matches(sql))
            {
                if (item.Groups.Count > 1) return item.Groups[1].Value;
            }
            throw new SqlParseException("未匹配到select……from语句！");
        }

        public string ReplaceFieldSql(string selectSql, string newSql)
        {
            var fieldListSql = GetFieldListSql(selectSql);
            return selectSql.Replace(fieldListSql, " " + newSql + " ");
        }

        public string GetRecompileSql(bool recompile)
        {
            return recompile ? " OPTION(RECOMPILE)" : "";
        }
    }
}
