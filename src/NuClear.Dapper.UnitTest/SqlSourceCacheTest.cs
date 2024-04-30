using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NuClear.Dapper.UnitTest
{
    public class SqlSourceCacheTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SameSourceTest()
        {
            var resource1 = new OrderSqlResource();
            var resource2 = new OrderSqlResource();
            var resource3 = new OrderSqlResource();

            var source1 = resource1.SqlSource;
            var source2 = resource2.SqlSource;
            var source3 = resource3.SqlSource;

            Assert.AreEqual(source1, source2);
            Assert.AreEqual(source1, source3);
        }

        [Test]
        public void MultiSourceTest()
        {
            var resource1 = new OrderSqlResource();
            var resource2 = new OrderSqlResource();

            var resource3 = new OrderSqlResource1();
            var resource4 = new OrderSqlResource1();

            var source1 = resource1.SqlSource;
            var source2 = resource2.SqlSource;
            var source3 = resource3.SqlSource;
            var source4 = resource4.SqlSource;

            Assert.AreEqual(source1, source2);
            Assert.AreEqual(source3, source4);
            Assert.AreNotEqual(source1, source3);
        }

    }
}
