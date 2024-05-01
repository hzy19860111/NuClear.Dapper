using NuClear.Dapper.Exceptions;
using System;
using System.Collections.Generic;

namespace NuClear.Dapper.AggregateQueryObject
{
    public class AggrQuery
    {
        private readonly List<AggrCriterion> _criterions = new List<AggrCriterion>();
        private dynamic result;
        public IEnumerable<AggrCriterion> Criterions { get { return _criterions; } }

        public AggrQuery Add(AggrCriterion criterion)
        {
            _criterions.Add(criterion);
            return this;
        }

        public void SetResult(dynamic obj)
        {
            this.result = obj;
        }

        public static AggrQuery Create()
        {
            return new AggrQuery();
        }

        public T GetValue<T>(int index)
        {
            if (index > this._criterions.Count - 1)
                throw new ArgumentOutOfRangeException("index");
            var criterion = this._criterions[index];
            return GetValueFromDynamicObject<T>(criterion);
        }

        private T GetValueFromDynamicObject<T>(AggrCriterion criterion)
        {
            if (typeof(T) != criterion.ValueType)
                throw new SqlParseException("获取类型与定义类型不同！");
            var dic = result as IDictionary<string, object>;
            return (T)Convert.ChangeType(dic[criterion.ResultPropertyName], criterion.ValueType);
        }
    }
}
