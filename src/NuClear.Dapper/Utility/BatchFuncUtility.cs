using System;
using System.Collections.Generic;
using System.Linq;

namespace NuClear.Dapper.Utility
{
    /// <summary>
    /// 根据数组 分页处理业务 工具类（一般用作dapper 根据in查询，dapper in 查询将数组 拼接为 参数，SqlServer最多支持2000）
    /// </summary>
    public static class BatchFuncUtility
    {
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <typeparam name="TResult">返回模型</typeparam>
        /// <param name="batchSource">操作数组源</param>
        /// <param name="func">具体操作，返回TResult集合</param>
        /// <param name="batchCount">一次批次性操作数量</param>
        /// <returns></returns>
        public static List<TResult> BatchFunc<TResult>(string[] batchSource, Func<string[], List<TResult>> func, int batchCount = 200)
        {

            var filterNullBatchSource = batchSource?.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            if (filterNullBatchSource == null || !filterNullBatchSource.Any())
            {
                return new List<TResult>(0);
            }

            IEnumerable<TResult> result = null;
            for (int i = 0; i < (filterNullBatchSource.Length - 1) / batchCount + 1; i++)
            {
                if (result == null)
                {
                    result = func(filterNullBatchSource.Skip(i * batchCount).Take(batchCount).ToArray());
                }
                else
                {
                    result = result.Union(func(filterNullBatchSource.Skip(i * batchCount).Take(batchCount).ToArray()));
                }
            }

            return result.ToList();
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <typeparam name="TResult">返回模型</typeparam>
        /// <param name="batchSource">操作数组源</param>
        /// <param name="func">具体操作，返回TResult集合</param>
        /// <param name="batchCount">一次批次性操作数量</param>
        /// <returns></returns>
        public static List<TResult> BatchFuncGenericSource<TSrouce, TResult>(TSrouce[] batchSource, Func<TSrouce[], List<TResult>> func, int batchCount = 200)
        {
            var filterNullBatchSource = batchSource?.ToArray();

            if (filterNullBatchSource == null || !filterNullBatchSource.Any())
            {
                return new List<TResult>(0);
            }

            IEnumerable<TResult> result = null;
            for (int i = 0; i < (filterNullBatchSource.Length - 1) / batchCount + 1; i++)
            {
                if (result == null)
                {
                    result = func(filterNullBatchSource.Skip(i * batchCount).Take(batchCount).ToArray());
                }
                else
                {
                    result = result.Union(func(filterNullBatchSource.Skip(i * batchCount).Take(batchCount).ToArray()));
                }
            }

            return result.ToList();
        }


        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="batchSource">操作数组源</param>
        /// <param name="action">具体操作</param>
        /// <param name="batchCount">一次批次性操作数量</param>
        /// <returns></returns>
        public static void BatchAction<T>(T[] batchSource, Action<T[]> action, int batchCount = 200) where T : class
        {

            var filterNullBatchSource = batchSource?.Where(s => s != null).ToArray();

            if (filterNullBatchSource == null || !filterNullBatchSource.Any())
            {
                return;
            }

            for (int i = 0; i < (filterNullBatchSource.Length - 1) / batchCount + 1; i++)
            {
                action(filterNullBatchSource.Skip(i * batchCount).Take(batchCount).ToArray());
            }
        }
    }
}
