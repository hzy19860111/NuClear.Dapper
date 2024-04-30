using NUnit.Framework;
using ognl;
using NuClear.Dapper.SqlResources.Scripting;

namespace NuClear.Dapper.UnitTest
{
    public class ExpressionEvaluatorTest
    {
        private ExpressionEvaluator evaluator;

        [SetUp]
        public void Setup()
        {
            OgnlRuntime.setPropertyAccessor(typeof(object), new ContextAccessor());

            evaluator = new ExpressionEvaluator();
        }

        [Test]
        public void ExpressionEvaluatorStringTest()
        {
            var test = "StoreHouseId != null and StoreHouseId != ''";
            var obj = new { StoreHouseId = "" };
            var obj1 = new { StoreHouseId = "123" };


            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj));
            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj1));
        }

        [Test]
        public void ExpressionEvaluatorIntTest()
        {
            var test = "SaleMode == 99";
            var test1 = "SaleMode == null";

            var obj = new { SaleMode = 99 };

            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj));
            Assert.IsFalse(evaluator.EvaluateBoolean(test1, obj));


            var obj1 = new { aaa = "1" };

            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj1));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, obj1));
        }


        [Test]
        public void ExpressionEvaluatorArrayTest()
        {
            var test = "StoreHouseIds != null and StoreHouseIds.Length > 0";
            var test1 = "StoreHouseIds == null";

            var obj = new { };

            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, obj));


            var obj1 = new { StoreHouseIds = new string[] { "1234" } };

            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj1));
            Assert.IsFalse(evaluator.EvaluateBoolean(test1, obj1));
        }

        [Test]
        public void ExpressionEvaluatorClassTest()
        {
            var test = "StoreHouseIds != null && StoreHouseIds.Length > 0";
            var test1 = "StoreHouseId == '123' && SaleMode == 99";

            var search = new Search();

            Assert.IsFalse(evaluator.EvaluateBoolean(test, search));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, search));
        }
    }

    public class Search
    {
        public string StoreHouseId { get; set; } = "123";
        public int SaleMode { get; set; } = 99;

        public string[] StoreHouseIds { get; set; }
    }

}