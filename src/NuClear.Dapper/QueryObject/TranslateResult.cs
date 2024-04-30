using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using NuClear.Dapper.Extensions;

namespace NuClear.Dapper.QueryObject
{
    public class TranslateResult
    {
        public TranslateResult()
        {
            this.SqlParameters = new List<SqlParameter>();
        }
        public string Sql { get; set; }
        public List<SqlParameter> SqlParameters { get; set; }

        public object Parameter { get { return this.SqlParameters.ToDynamicObject(); } }
    }
}
