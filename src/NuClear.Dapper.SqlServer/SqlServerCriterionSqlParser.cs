using NuClear.Dapper.QueryObject;
using System;

namespace NuClear.Dapper.SqlServer
{
    public class SqlServerCriterionSqlParser : ICriterionSqlParser
    {
        public string ToSql(Criterion criterion)
        {
            string oper = criterion.Operator.ToSqlOperator();

            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            if (oper == null && !string.IsNullOrWhiteSpace(fixedKeywordPropertyName))
            {
                return fixedKeywordPropertyName;
            }

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";
            var tableAndPrepertyName = $"{aliasName}{fixedKeywordPropertyName}";

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return $"{tableAndPrepertyName} {oper} '%' + @{criterion.PropertyParameterName} + '%' ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.LeftLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return $"{tableAndPrepertyName} {oper} '%' + @{criterion.PropertyParameterName} ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.RightLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());
                return $"{tableAndPrepertyName} {oper} @{criterion.PropertyParameterName} + '%' ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                return $"{tableAndPrepertyName} {oper}";
            }
            return $"{tableAndPrepertyName} {oper} @{criterion.PropertyParameterName}";
        }

        private string GetESCAPEString(object value)
        {
            return value.ToString()
                     .Replace("~", "~~")
                     .Replace("[", "~[")
                     .Replace("%", "~%")
                     .Replace("_", "~_")
                     .Replace("'", "''");
        }

        public string ToTableValueSql(Criterion criterion)
        {
            string oper = criterion.Operator.ToSqlOperator();
            if (oper == null)
            {
                return null;
            }

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                throw new NotSupportedException("表条件不支持 Like和NotLike操作符！");
            }

            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                throw new NotSupportedException("表条件不支持 IsNotNull和IsNull操作符！");
            }

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : $"{criterion.TableAliasName}.";

            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            return $"{aliasName}{fixedKeywordPropertyName} {oper} {criterion.Value}";
        }

        private string FixedKeywordPropertyName(string propertyName)
        {
            return SqlUtility.HandleSqlServerKeyword(propertyName);
        }
    }
}
