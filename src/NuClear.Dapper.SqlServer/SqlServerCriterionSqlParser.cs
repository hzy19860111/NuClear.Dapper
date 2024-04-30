using System;
using NuClear.Dapper.QueryObject;

namespace NuClear.Dapper.SqlServer
{
    public class SqlServerCriterionSqlParser : ICriterionSqlParser
    {
        public string ToSql(Criterion criterion)
        {
            string oper = this.ToSqlOperator(criterion.Operator);
            //if (oper == null) return null;

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";
            var fixedKeywordPropertyName = FixedKeywordPropertyName(criterion.PropertyName);

            if (oper == null && !string.IsNullOrWhiteSpace(fixedKeywordPropertyName))
            {
                return string.Format("{0}", fixedKeywordPropertyName);
            }

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return string.Format("{0}{1} {2} '%' + @{3} + '%' ESCAPE N'~'", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
            }

            if (criterion.Operator == CriteriaOperator.LeftLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return string.Format("{0}{1} {2} '%' + @{3} ESCAPE N'~'", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
            }

            if (criterion.Operator == CriteriaOperator.RightLike)
            {
                criterion.Value = GetESCAPEString(criterion.Value.ToString());

                return string.Format("{0}{1} {2} @{3} + '%' ESCAPE N'~'", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
            }

            if (criterion.Operator == CriteriaOperator.In || criterion.Operator == CriteriaOperator.NotIn)
            {
                return string.Format("{0}{1} {2} @{3}", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
            }
            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                return string.Format("{0}{1} {2}", aliasName, fixedKeywordPropertyName, oper);
            }

            return string.Format("{0}{1}{2}@{3}", aliasName, fixedKeywordPropertyName, oper, criterion.PropertyParameterName);
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
            string oper = this.ToSqlOperator(criterion.Operator);
            if (oper == null) return null;

            var aliasName = string.IsNullOrWhiteSpace(criterion.TableAliasName) ? "" : criterion.TableAliasName + ".";

            if (criterion.Operator == CriteriaOperator.Like || criterion.Operator == CriteriaOperator.NotLike)
            {
                throw new NotSupportedException("表条件不支持 Like和NotLike操作符！");
            }

            if (criterion.Operator == CriteriaOperator.In || criterion.Operator == CriteriaOperator.NotIn)
            {
                return string.Format("{0}{1} {2} {3}", aliasName, FixedKeywordPropertyName(criterion.PropertyName), oper, criterion.Value);
            }
            if (criterion.Operator == CriteriaOperator.IsNotNull || criterion.Operator == CriteriaOperator.IsNull)
            {
                throw new NotSupportedException("表条件不支持 IsNotNull和IsNull操作符！");
            }

            return string.Format("{0}{1}{2}{3}", aliasName, FixedKeywordPropertyName(criterion.PropertyName), oper, criterion.Value);
        }

        private string FixedKeywordPropertyName(string propertyName)
        {
            return SqlUtility.HandleSqlServerKeyword(propertyName);
        }


        public string ToSqlOperator(CriteriaOperator criteriaOperator)
        {
            switch (criteriaOperator)
            {
                case CriteriaOperator.Equal:
                    return "=";
                case CriteriaOperator.GreaterThan:
                    return ">";
                case CriteriaOperator.GreaterThanOrEqual:
                    return ">=";
                case CriteriaOperator.LessThen:
                    return "<";
                case CriteriaOperator.LessThenOrEqual:
                    return "<=";
                case CriteriaOperator.Like:
                case CriteriaOperator.LeftLike:
                case CriteriaOperator.RightLike:
                    return "like";
                case CriteriaOperator.NotLike:
                    return "not like";
                case CriteriaOperator.In:
                    return "in";
                case CriteriaOperator.NotIn:
                    return "not in";
                case CriteriaOperator.NotEqual:
                    return "<>";
                case CriteriaOperator.IsNull:
                    return "IS NULL";
                case CriteriaOperator.IsNotNull:
                    return "IS NOT NULL";
                default:
                    return null;
            }
        }

    }
}
