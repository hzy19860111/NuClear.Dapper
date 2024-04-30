namespace NuClear.Dapper.SqlResources.Scripting
{
    public interface ISqlNode
    {
        bool Apply(DynamicContext context);
    }
}
