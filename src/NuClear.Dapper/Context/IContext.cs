using System;
using System.Data;

namespace NuClear.Dapper.Context
{
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// 数据库事务
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// 是否已开启事务
        /// </summary>
        bool IsTransactionStarted { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();
    }
}
