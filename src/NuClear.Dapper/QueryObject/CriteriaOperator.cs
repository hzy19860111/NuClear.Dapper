namespace NuClear.Dapper.QueryObject
{
    public enum CriteriaOperator
    {
        None = 0,
        Equal = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThen = 4,
        LessThenOrEqual = 5,
        Like = 6,
        In = 7,
        NotIn = 8,
        NotEqual = 9,
        IsNull = 10,
        IsNotNull = 11,
        NotLike = 12,
        LeftLike = 13,
        RightLike = 14
    }
}
