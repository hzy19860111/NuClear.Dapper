using System;
using NuClear.Dapper.Context;

namespace NuClear.Dapper
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : IContext
    {
        internal readonly TContext _context;
        public UnitOfWork(TContext context)
        {
            this._context = context;
        }

        public TContext Context { get { return _context; } }

        public IUnitOfWorkCompleteHandle Begin()
        {
            if (_context.IsTransactionStarted)
            {
                throw new InvalidOperationException("已开启事务.");
            }
            _context.BeginTransaction();
            return new UnitOfWorkCompleteHandle<TContext>(this);
        }
        public void Commit()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("事务已提交或被释放.");

            _context.Commit();
        }
        public void Rollback()
        {
            if (!_context.IsTransactionStarted)
                throw new InvalidOperationException("当前无事务.");
            _context.Rollback();
        }

        #region IDisposed

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class UnitOfWork : UnitOfWork<IContext>, IUnitOfWork<IContext>, IUnitOfWork
    {
        public UnitOfWork(IContext context) : base(context)
        {
        }
    }
}
