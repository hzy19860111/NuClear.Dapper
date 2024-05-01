using System;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.SqlServer
{
    public class SqlServerCriterionSqlParser : ICriterionSqlParser
    {
        public string ToSql(Criterion criterion)
        {
            string oper = criterion.Operator.ToSqlOperator();

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";
            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            if (oper == null && !string.IsNullOrWhiteSpace(fixedKeywordPropertyName))
            {
                return fixedKeywordPropertyName;
            }

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return $"{aliasName}{fixedKeywordPropertyName} {oper} '%' + @{criterion.PropertyParameterName} + '%' ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.LeftLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return $"{aliasName}{fixedKeywordPropertyName} {oper} '%' + @{criterion.PropertyParameterName} ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.RightLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());
                return $"{aliasName}{fixedKeywordPropertyName} {oper} @{criterion.PropertyParameterName} + '%' ESCAPE N'~'";
            }

            if (criterion.Operator == CriteriaOperator.In || criterion.Operator == CriteriaOperator.NotIn)
            {
                return $"{aliasName}{fixedKeywordPropertyName} {oper} @{criterion.PropertyParameterName}";
            }
            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                return $"{aliasName}{fixedKeywordPropertyName} {oper}";
            }
            return $"{aliasName}{fixedKeywordPropertyName}{oper}@{criterion.PropertyParameterName}";
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
            if (oper == null) return null;

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : $"{criterion.TableAliasName}.";

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                throw new NotSupportedException("表条件不支持 Like和NotLike操作符！");
            }

            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                throw new NotSupportedException("表条件不支持 IsNotNull和IsNull操作符！");
            }

            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            if (criterion.Operator == CriteriaOperator.In || criterion.Operator == CriteriaOperator.NotIn)
            {
                return $"{aliasName}{fixedKeywordPropertyName} {oper} {criterion.Value}";
            }

            return $"{aliasName}{fixedKeywordPropertyName}{oper}{criterion.Value}";
        }

        private string FixedKeywordPropertyName(string propertyName)
        {
            return SqlUtility.HandleSqlServerKeyword(propertyName);
        }
    }
}
