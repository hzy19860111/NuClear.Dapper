namespace NuClear.Dapper.QueryObject
{
    public class Criterion
    {
        /// <summary>
        /// 属性名字
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// 参数属性名（正常 与PropertyName一致，当有多个同一属性的查询条件时，需重新定义）
        /// </summary>
        public string PropertyParameterName { get; private set; }

        public CriteriaOperator Operator { get; private set; }

        public object Value { get; set; }

        /// <summary>
        /// 表别名 默认为空
        /// </summary>
        public string TableAliasName { get; private set; }

        private Criterion(string propertyName, object value, CriteriaOperator criteriaOperator, string tableAliasName)
        {
            PropertyName = propertyName;
            Value = value;
            Operator = criteriaOperator;
            TableAliasName = tableAliasName;
            PropertyParameterName = propertyName;
        }

        internal void RenamePropertyName(string renamePropertyName)
        {
            PropertyParameterName = renamePropertyName;
        }

        public static Criterion Create(string propertyName, object value, CriteriaOperator criteriaOperator, string tableAliasName = null, bool isTableValue = false)
        {
            if (isTableValue)
                return new TableValueCriterion(propertyName, value, criteriaOperator, tableAliasName);

            return new Criterion(propertyName, value, criteriaOperator, tableAliasName);
        }

        internal class TableValueCriterion : Criterion
        {
            internal TableValueCriterion(string propertyName, object value, CriteriaOperator criteriaOperator, string tableAliasName)
                : base(propertyName, value, criteriaOperator, tableAliasName)
            {
            }
        }
    }
}
