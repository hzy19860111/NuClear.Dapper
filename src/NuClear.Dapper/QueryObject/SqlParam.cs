namespace NuClear.Dapper.QueryObject
{
    public class SqlParam
    {
        public SqlParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }
    }
}
