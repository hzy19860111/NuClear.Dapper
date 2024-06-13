using NUnit.Framework;
using System.Dynamic;

namespace NuClear.Dapper.UnitTest
{
    public class XmlFileSqlResourceTest
    {
        private OrderSqlResource sqlResource;
        [SetUp]
        public void Setup()
        {
            sqlResource = new OrderSqlResource();
        }


        [Test]
        public void TextSqlTest()
        {
            var xmlSql = "SELECT [Id],[OrderNo],[OrderType],[Amount] FROM [dbo].[Orders] WITH(NOLOCK)";
            var selectSql = sqlResource.GetSql("Select");

            Assert.IsNotEmpty(selectSql);
            Assert.AreEqual(xmlSql, selectSql);
        }




        [Test]
        public void CDataSqlTest()
        {
            var xmlSql = "SELECT [Id],[OrderNo],[OrderType],[Amount] FROM [dbo].[Orders] WITH(NOLOCK)";
            var selectSql = sqlResource.GetSql("Select1");

            Assert.IsNotEmpty(selectSql);
            Assert.AreEqual(xmlSql, selectSql);
        }




        [Test]
        public void ObjectParameterTest()
        {
            string xmlSql = @"SELECT * FROM dbo.OrderItems oi WITH(NOLOCK)    INNER JOIN dbo.Orders o WITH(NOLOCK) ON oi.Order_Id = o.Id   AND oi.OrderType IN (0,1,2) OPTION(RECOMPILE)";

            object obj = new
            {
                OrderType = 2,
                Id = "",
                Ids = new string[0] { },
            };

            var sql = sqlResource.GetSql("SelectOrderItems", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void DynamicParameterTest()
        {
            string xmlSql = @"SELECT * FROM dbo.OrderItems oi WITH(NOLOCK)    INNER JOIN dbo.Orders o WITH(NOLOCK) ON oi.Order_Id = o.Id   AND oi.OrderType IN (0,1,2) AND  o.Id in @Ids OPTION(RECOMPILE)";

            dynamic obj = new ExpandoObject();
            obj.OrderType = 2;
            obj.SettlementType = null;
            obj.Id = "";
            obj.IdsCount = 1;
            obj.Ids = new string[] { "1111" };

            var sql = sqlResource.GetSql("SelectOrderItems", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void IfAllParameterTest()
        {
            string xmlSql = @"SELECT * FROM dbo.OrderItems oi WITH(NOLOCK)    INNER JOIN dbo.Orders o WITH(NOLOCK) ON oi.Order_Id = o.Id   AND oi.OrderType IN (0,1,2) AND  @Id=o.Id AND  o.Id in @Ids OPTION(RECOMPILE)";

            dynamic obj = new ExpandoObject();
            obj.OrderType = 2;
            obj.Id = "123";
            obj.Ids = new string[] { "1111" };


            var sql = sqlResource.GetSql("SelectOrderItems", obj);

            Assert.AreEqual(xmlSql, sql);
        }




        [Test]
        public void ChooseWhen1Test()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and Id = @Id";

            object obj = new { Id = 2 };

            var sql = sqlResource.GetSql("SelectByChoose", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void ChooseWhen2Test()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and OrderNo = @OrderNo";

            object obj = new { OrderNo = "111" };
            var sql = sqlResource.GetSql("SelectByChoose", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void ChooseOtherwiseTest()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and BusinessTime >= '2022-8-1'";

            //otherwise
            object obj = new { };

            var sql = sqlResource.GetSql("SelectByChoose", obj);

            Assert.AreEqual(xmlSql, sql);
        }

        [Test]
        public void ChooseIfTest()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and OrderNo=@OrderNo and UserStatus = 1";

            object obj = new { OrderNo = "111", SelectHiden = 0 };
            var sql = sqlResource.GetSql("SelectByChooseAndIf", obj);
            Assert.AreEqual(xmlSql, sql);


            string xmlSql1 = @"select * from Orders o with(nolock)    where 1 = 1 and OrderNo=@OrderNo";

            object obj1 = new { OrderNo = "111", SelectHiden = 1 };
            var sql1 = sqlResource.GetSql("SelectByChooseAndIf", obj1);
            Assert.AreEqual(xmlSql1, sql1);
        }
    }
}
