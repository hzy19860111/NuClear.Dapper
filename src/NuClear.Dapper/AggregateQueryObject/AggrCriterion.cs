using System;

namespace NuClear.Dapper.AggregateQueryObject
{
    public class AggrCriterion
    {
        private AggrCriterion(string propertyName, AggrCriteriaOperator criteriaOperator, string tableAliasName, Type valueType)
        {
            PropertyName = propertyName;
            Operator = criteriaOperator;
            TableAliasName = tableAliasName;
            ValueType = valueType;
        }

        /// <summary>
        /// 属性名字
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// 结果属性名称
        /// </summary>
        public string ResultPropertyName { get; set; }

        public AggrCriteriaOperator Operator { get; private set; }

        public Type ValueType { get; private set; }

        /// <summary>
        /// 表别名 默认为空
        /// </summary>
        public string TableAliasName { get; private set; }

        public static AggrCriterion Create<T>(string propertyName, AggrCriteriaOperator criteriaOperator, string tableAliasName = null)
        {
            return new AggrCriterion(propertyName, criteriaOperator, tableAliasName, typeof(T));
        }
    }
}
