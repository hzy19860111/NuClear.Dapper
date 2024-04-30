namespace NuClear.Dapper.QueryObject
{
    public interface ICriterionSqlParser
    {
        /// <summary>
        /// 解析criterion为sql语句
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        string ToSql(Criterion criterion);
        /// <summary>
        /// 解析表条件criterion为sql 语句
        /// </summary>
        /// <param name="criterion"></param>
        /// <returns></returns>
        string ToTableValueSql(Criterion criterion);
    }
}
