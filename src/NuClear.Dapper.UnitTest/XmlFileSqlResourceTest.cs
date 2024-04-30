using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

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
            var xmlSql = "SELECT [Id],[OrderNo],[OmsOrderId],[OmsOrderNo],OwnerId,SecOwnerId,[HasCreateNote],[OrderType],[PayType],[Bonus],[Coupons]FROM [dbo].[Orders] WITH(NOLOCK)";
            var selectSql = sqlResource.GetSql("Select");

            Assert.IsNotEmpty(selectSql);
            Assert.AreEqual(xmlSql, selectSql);
        }




        [Test]
        public void CDataSqlTest()
        {
            var xmlSql = "SELECT [Id],[OrderNo],[OmsOrderId],[OmsOrderNo],OwnerId,SecOwnerId,[HasCreateNote],[OrderType],[PayType],[Bonus],[Coupons]FROM [dbo].[Orders] WITH(NOLOCK)";
            var selectSql = sqlResource.GetSql("Select1");

            Assert.IsNotEmpty(selectSql);
            Assert.AreEqual(xmlSql, selectSql);
        }




        [Test]
        public void ObjectParameterTest()
        {
            string xmlSql = @"SELECT *    FROM dbo.NoteItems ni WITH(NOLOCK)    INNER JOIN dbo.OrderOutStockNote o WITH(NOLOCK) ON ni.OrderOutStockNote_Id = o.Id    AND o.City_Id=@CityId    AND o.AuditState=9  AND o.IsDeleted =0 AND o.UserStatus = 0    AND (YEAR(o.BusinessTime)=@Year AND MONTH(o.BusinessTime)=@Month) AND ni.SaleMode IN (1,3,9) OPTION(RECOMPILE)";

            object obj = new
            {
                SaleMode = 99,
                StoreHouseId = "",
                StoreHouseIds = new string[0] { },
            };

            var sql = sqlResource.GetSql("QueryPagedSaleProfitByMonth", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void DynamicParameterTest()
        {
            string xmlSql = @"SELECT *    FROM dbo.NoteItems ni WITH(NOLOCK)    INNER JOIN dbo.OrderOutStockNote o WITH(NOLOCK) ON ni.OrderOutStockNote_Id = o.Id    AND o.City_Id=@CityId    AND o.AuditState=9  AND o.IsDeleted =0 AND o.UserStatus = 0    AND (YEAR(o.BusinessTime)=@Year AND MONTH(o.BusinessTime)=@Month) AND ni.SaleMode IN (1,3,9) AND  o.StoreHouse_Id in @StoreHouseIds OPTION(RECOMPILE)";

            dynamic obj = new ExpandoObject();
            obj.SaleMode = 99;
            obj.SettlementType = null;
            obj.StoreHouseId = "";
            obj.StoreHouseIdsCount = 1;
            obj.StoreHouseIds = new string[] { "1111" };

            var sql = sqlResource.GetSql("QueryPagedSaleProfitByMonth", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void IfAllParameterTest()
        {
            string xmlSql = @"SELECT *    FROM dbo.NoteItems ni WITH(NOLOCK)    INNER JOIN dbo.OrderOutStockNote o WITH(NOLOCK) ON ni.OrderOutStockNote_Id = o.Id    AND o.City_Id=@CityId    AND o.AuditState=9  AND o.IsDeleted =0 AND o.UserStatus = 0    AND (YEAR(o.BusinessTime)=@Year AND MONTH(o.BusinessTime)=@Month) AND ni.SaleMode IN (1,3,9) AND @SettlementType = ni.SettlementType AND  @FinancialBusinessType=o.FinancialBusinessType AND  @StoreHouseId=o.StoreHouse_Id AND  o.StoreHouse_Id in @StoreHouseIds OPTION(RECOMPILE)";

            dynamic obj = new ExpandoObject();
            obj.SaleMode = 99;
            obj.FinancialBusinessType = 1;
            obj.SettlementType = 1;
            obj.StoreHouseId = "123";
            obj.StoreHouseIds = new string[] { "1111" };


            var sql = sqlResource.GetSql("QueryPagedSaleProfitByMonth", obj);

            Assert.AreEqual(xmlSql, sql);
        }




        [Test]
        public void ChooseWhen1Test()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and Id = @Id";

            object obj = new { Id = 99 };

            var sql = sqlResource.GetSql("SelectByChoose", obj);

            Assert.AreEqual(xmlSql, sql);
        }


        [Test]
        public void ChooseWhen2Test()
        {
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and NoteNo = @NoteNo";

            object obj = new { NoteNo = "111" };
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
            string xmlSql = @"select * from Orders o with(nolock)    where 1 = 1 and noteno=@NoteNo and UserStatus = 1";

            object obj = new { NoteNo = "111", SelectHiden = 0 };
            var sql = sqlResource.GetSql("SelectByChooseAndIf", obj);
            Assert.AreEqual(xmlSql, sql);


            string xmlSql1 = @"select * from Orders o with(nolock)    where 1 = 1 and noteno=@NoteNo";

            object obj1 = new { NoteNo = "111", SelectHiden = 1 };
            var sql1 = sqlResource.GetSql("SelectByChooseAndIf", obj1);
            Assert.AreEqual(xmlSql1, sql1);
        }
    }
}
