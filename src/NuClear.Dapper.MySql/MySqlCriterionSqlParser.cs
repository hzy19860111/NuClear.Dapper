using System;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.MySql
{
    public class MySqlCriterionSqlParser : ICriterionSqlParser
    {
        public string ToSql(Criterion criterion)
        {
            string oper = criterion.Operator.ToSqlOperator();

            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            if (oper == null && !string.IsNullOrWhiteSpace(fixedKeywordPropertyName))
            {
                return string.Format("{0}", fixedKeywordPropertyName);
            }

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";

            var tableAndPrepertyName = $"{aliasName}{fixedKeywordPropertyName}";

            if (criterion.Operator.IsLikeOperator())
            {
                criterion.Value = criterion.Value.ToString().Replace(@"%", @"\%");

                if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
                {
                    return $"{tableAndPrepertyName} {oper} CONCAT('%', @{criterion.PropertyParameterName}, '%')";
                }

                if (criterion.Operator == CriteriaOperator.LeftLike)
                {
                    return $"{tableAndPrepertyName} {oper} CONCAT('%', @{criterion.PropertyParameterName})";
                }

                if (criterion.Operator == CriteriaOperator.RightLike)
                {
                    return $"{tableAndPrepertyName} {oper} CONCAT(@{criterion.PropertyParameterName}, '%')";
                }
            }


            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                return string.Format("{0}{1} {2}", aliasName, fixedKeywordPropertyName, oper);
            }

            return string.Format("{0}{1} {2} @{3}", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
        }


        public string ToTableValueSql(Criterion criterion)
        {
            string oper = criterion.Operator.ToSqlOperator();
            if (oper == null) return null;

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                throw new NotSupportedException("表条件不支持 Like和NotLike操作符！");
            }

            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                throw new NotSupportedException("表条件不支持 IsNotNull和IsNull操作符！");
            }

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";

            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            var tableAndPrepertyName = $"{aliasName}{fixedKeywordPropertyName}";

            if (criterion.Operator == CriteriaOperator.In || criterion.Operator == CriteriaOperator.NotIn)
            {
                return $"{tableAndPrepertyName} {oper} {criterion.Value}";
            }

            return $"{tableAndPrepertyName}{oper}{criterion.Value}";
        }

        private string FixedKeywordPropertyName(string propertyName)
        {
            return SqlUtility.HandleMySqlKeyword(propertyName);
        }
    }
}
