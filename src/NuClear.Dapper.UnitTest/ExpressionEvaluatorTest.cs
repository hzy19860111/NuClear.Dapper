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
            var test = "Id != null and Id != ''";
            var obj = new { Id = "" };
            var obj1 = new { Id = "123" };


            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj));
            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj1));
        }

        [Test]
        public void ExpressionEvaluatorIntTest()
        {
            var test = "OrderType == 2";
            var test1 = "OrderType == null";

            var obj = new { OrderType = 2 };

            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj));
            Assert.IsFalse(evaluator.EvaluateBoolean(test1, obj));


            var obj1 = new { aaa = "1" };

            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj1));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, obj1));
        }


        [Test]
        public void ExpressionEvaluatorArrayTest()
        {
            var test = "Ids != null and Ids.Length > 0";
            var test1 = "Ids == null";

            var obj = new { };

            Assert.IsFalse(evaluator.EvaluateBoolean(test, obj));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, obj));


            var obj1 = new { Ids = new string[] { "1234" } };

            Assert.IsTrue(evaluator.EvaluateBoolean(test, obj1));
            Assert.IsFalse(evaluator.EvaluateBoolean(test1, obj1));
        }

        [Test]
        public void ExpressionEvaluatorClassTest()
        {
            var test = "Ids != null && Ids.Length > 0";
            var test1 = "Id == '123' && OrderType == 2";

            var search = new Search();

            Assert.IsFalse(evaluator.EvaluateBoolean(test, search));
            Assert.IsTrue(evaluator.EvaluateBoolean(test1, search));
        }
    }

    public class Search
    {
        public string Id { get; set; } = "123";
        public int OrderType { get; set; } = 2;

        public string[] Ids { get; set; }
    }

}