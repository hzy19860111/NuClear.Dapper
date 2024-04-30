using System;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace NuClear.Dapper.Context
{
    public abstract class ContextBase : IContext
    {
        private bool _isTransactionStarted = false;
        protected IDbConnection _connection = null;
        private IDbTransaction _transaction = null;

        public string Id { get; }
        public bool IsTransactionStarted
        {
            get
            {
                return _isTransactionStarted;
            }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }



        protected abstract void CreateConnection();

        protected ContextBase()
        {
            Id = Guid.NewGuid().ToString("N");

            DebugPrint("Connection started.");
        }

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>> ContextBase - Thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }

        public void BeginTransaction()
        {
            if (_isTransactionStarted)
                throw new InvalidOperationException("已开启事务.");

            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            _transaction = _connection.BeginTransaction();

            _isTransactionStarted = true;

            DebugPrint("事务开启.");
        }

        public void Commit()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("当前无事务.");

            _transaction.Commit();
            _transaction = null;

            _isTransactionStarted = false;

            DebugPrint("事务已提交.");
        }


        public void Rollback()
        {
            if (!_isTransactionStarted)
                throw new InvalidOperationException("当前无事务.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            _isTransactionStarted = false;

            DebugPrint("事务已回滚.");
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                if (_isTransactionStarted)
                {
                    try { Rollback(); }
                    catch
                    {
                        DebugPrint("事务已开启，释放数据库连接回滚.");
                    }
                }
                _connection.Close();
                _connection.Dispose();
                _connection = null;

                DebugPrint("数据库连接已释放，事务已回滚.");
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
