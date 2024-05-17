using System.Collections.Generic;

namespace NuClear.Dapper.QueryObject
{
    public class TranslateResult
    {
        public TranslateResult()
        {
            this.SqlParameters = new List<SqlParam>();
        }
        public string Sql { get; set; }
        public List<SqlParam> SqlParameters { get; set; }

        public object Parameter { get { return this.SqlParameters.ToDynamicObject(); } }
    }
}
