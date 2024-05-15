using NuClear.Snowflake;

namespace NuClear.Dapper
{
    public class BaseEntityWithLongKey : BaseEntity<long>, IEntity
    {
        internal static readonly IdWorker idWorker = IdWorker.Create();
        public BaseEntityWithLongKey()
            : base()
        {
            Id = idWorker.NextId();
        }
    }
}