namespace NuClear.Dapper.AggregateQueryObject
{
    public interface IAggrQuerySqlParser
    {
        /// <summary>
        /// 将query解析为sql语句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        string Translate(AggrQuery query);
    }
}
