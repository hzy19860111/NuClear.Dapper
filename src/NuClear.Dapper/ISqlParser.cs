namespace NuClear.Dapper
{
    public interface ISqlParser
    {
        /// <summary>
        /// 将select语句解析为 count语句
        /// </summary>
        /// <param name="selectSql"></param>
        /// <returns></returns>
        string CreateCountSqlBySelectSql(string selectSql);
        /// <summary>
        ///  将给定语句组装为top 1 语句 
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="sortSql"></param>
        /// <returns></returns>
        string CreateTop1SelectSql(string selectSql, string whereSql = "", string sortSql = "");
        /// <summary>
        /// 将给定语句组装为分页sql
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="sortSql"></param>
        /// <returns></returns>
        string CreatePagedSql(string selectSql, string whereSql = null, string sortSql = null, Pager pager = null);
        /// <summary>
        /// 将select中的表字段替换为给定的新sql
        /// </summary>
        /// <param name="selectSql"></param>
        /// <param name="newSql"></param>
        /// <returns></returns>
        string ReplaceFieldSql(string selectSql, string newSql);
    }
}
