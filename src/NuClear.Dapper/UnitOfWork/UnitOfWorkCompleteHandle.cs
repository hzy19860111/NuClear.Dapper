using System;
using NuClear.Dapper.Context;

namespace NuClear.Dapper
{
    public class UnitOfWorkCompleteHandle<TContext> : IUnitOfWorkCompleteHandle
        where TContext : IContext
    {
        private readonly IUnitOfWork<TContext> _uow;
        public UnitOfWorkCompleteHandle(IUnitOfWork<TContext> uow)
        {
            _uow = uow;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_uow.Context.IsTransactionStarted)
                    {
                        try
                        {
                            _uow.Rollback();
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class UnitOfWorkCompleteHandle : IUnitOfWorkCompleteHandle
    {
        private readonly IUnitOfWork _uow;
        public UnitOfWorkCompleteHandle(IUnitOfWork uow)
        {
            _uow = uow;
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_uow.Context.IsTransactionStarted)
                    {
                        try
                        {
                            _uow.Rollback();
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                }
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
