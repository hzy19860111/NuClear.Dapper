namespace NuClear.Dapper.SqlResources.Scripting
{
    public interface ISqlSource
    {
        string GetSql(string key, object parameterObject = null);
    }
}
