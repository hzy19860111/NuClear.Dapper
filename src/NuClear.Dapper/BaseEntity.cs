using System;

namespace NuClear.Dapper
{
    /// <summary>
    /// 所有实体的基类
    /// </summary>
    public class BaseEntity<TKey> : IEntity<TKey>
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
            LastUpdateTime = DateTime.Now;
        }


        public TKey Id { get; set; }

        /// <summary>
        /// 数据的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}