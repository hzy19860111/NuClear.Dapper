using System;
using NuClear.Dapper.Context;

namespace NuClear.Dapper
{
    public interface IUnitOfWork : IDisposable
    {
        IContext Context { get; }
        IUnitOfWorkCompleteHandle Begin();
        void Commit();
        void Rollback();
    }

    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : IContext
    {
        TContext Context { get; }
        IUnitOfWorkCompleteHandle Begin();
        void Commit();
        void Rollback();
    }
}
