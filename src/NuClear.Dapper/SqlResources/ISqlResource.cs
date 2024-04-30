namespace NuClear.Dapper.SqlResources
{
    public interface ISqlResource<T>
            where T : IEntity
    {
        /// <summary>
        /// 获取sql语句
        /// </summary>
        /// <param name="key">sql语句唯一Key</param>
        /// <returns></returns>
        string GetSql(string key);
        /// <summary>
        /// 根据入参获取sql语句
        /// </summary>
        /// <param name="key">sql语句唯一Key</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        string GetSql(string key, object parameterObject);
        /// <summary>
        /// 查询语句
        /// </summary>
        string Select { get; }
        /// <summary>
        /// 插入语句
        /// </summary>
        string Insert { get; }
        /// <summary>
        /// 更新语句
        /// </summary>
        string Update { get; }
        /// <summary>
        /// 删除语句
        /// </summary>
        string Delete { get; }
    }
}
