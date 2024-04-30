using NuClear.Snowflake;
using System;

namespace NuClear.Dapper
{
    /// <summary>
    /// 所有实体的基类
    /// </summary>
    public class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString("N");

            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public string Id { get; set; }

        /// <summary>
        /// 数据的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }

    public class BaseEntity<T> : IEntity
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public T Id { get; set; }

        /// <summary>
        /// 数据的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        string IEntity.Id
        {
            get
            {
                return this.Id.ToString();
            }
            set
            {
                T ret = (T)Convert.ChangeType("-1", typeof(T));

                try
                {
                    ret = (T)Convert.ChangeType(value, typeof(T));
                }
                catch { }

                this.Id = ret;
            }
        }
    }

    public class BaseEntityNum : IEntity
    {
        public BaseEntityNum()
        {
            Id = IdGenerator.NextId();

            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }

        public long Id { get; set; }
        string IEntity.Id { get => Id.ToString(); set => Convert.ToUInt64(Id); }
        /// <summary>
        /// 数据的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }

    public class BaseEntityMySql : IEntity
    {
        public BaseEntityMySql()
        {
            Id = IdGenerator.NextId();

            CreateTimeTxt = DateTime.Now;
            CreateTime = CreateTimeTxt.ToTimeStamp();
            LastUpdateTimeTxt = DateTime.Now;
            LastUpdateTime = LastUpdateTimeTxt.ToTimeStamp();
        }

        public long Id { get; set; }
        string IEntity.Id { get => Id.ToString(); set => Convert.ToUInt64(Id); }
        /// <summary>
        /// 数据的创建时间
        /// </summary>
        public DateTime CreateTimeTxt { get; set; }
        public long CreateTime { get; set; }


        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTimeTxt { get; set; }
        public long LastUpdateTime { get; set; }
    }

    public static class IdGenerator
    {
        private static IdWorker idWorker = new IdWorker(1, 1);

        public static long NextId()
        {
            return idWorker.NextId();
        }

        public static long ToTimeStamp(this DateTime time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                TimeZoneInfo.Local);
            long stamp = (time.Ticks - startTime.Ticks) / 10000; //除10000调整为13位
            return stamp;
        }
    }



}