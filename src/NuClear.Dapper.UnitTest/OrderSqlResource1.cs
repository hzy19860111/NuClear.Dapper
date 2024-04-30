using System;
using System.Collections.Generic;
using System.Text;
using NuClear.Dapper.SqlResources;

namespace NuClear.Dapper.UnitTest
{
    public class OrderSqlResource1 : XmlFileBaseSqlResource<TestOrder>
    {
        protected override string Directory => AppDomain.CurrentDomain.BaseDirectory;

        public override string FileName => "OrderSqlResource1";
    }
}
