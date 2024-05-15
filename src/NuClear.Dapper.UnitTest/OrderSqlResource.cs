using NuClear.Dapper.SqlResources;
using System;

namespace NuClear.Dapper.UnitTest
{
    public class OrderSqlResource : XmlFileBaseSqlResource<TestOrder>
    {
        protected override string Directory => AppDomain.CurrentDomain.BaseDirectory;

        public override string FileName => "OrderSqlResource";
    }
}
