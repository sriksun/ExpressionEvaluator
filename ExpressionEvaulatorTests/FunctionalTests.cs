using System;
using ExpressionEvaluator;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class FunctionalTests
    {
        private static object[] IntCases = new object[]
        {
            new object[] { "a", 5, 5, 5 },
            new object[] { "a+1", 5, 6, 5 },
            new object[] { "a-1", 5, 4, 5 },
            new object[] { "a*2", 5, 10, 5 },
            new object[] { "a/2", 10, 5, 10 },
            new object[] { "a % 2", 5, 1, 5 },
            new object[] { "a << 1", 5, 10, 5 },
            new object[] { "a >> 1", 5, 2, 5 },
            new object[] { "a & 7", 5, 5, 5 },
            new object[] { "a | 2", 5, 7, 5 },
            new object[] { "a ^ a", 5, 0, 5 },
            new object[] { "a+=1", 5, 6, 6 },
            new object[] { "a-=1", 5, 4, 4 },
            new object[] { "a*=2", 5, 10, 10 },
            new object[] { "a/=2", 10, 5, 5 },
            new object[] { "a%=2", 10, 0, 0 },
            new object[] { "a<<=1", 5, 10, 10 },
            new object[] { "a>>=1", 10, 5, 5 },
            new object[] { "a&=7", 5, 5, 5 },
            new object[] { "a|=7", 10, 15, 15 },
            new object[] { "a^=7", 5, 2, 2 },
            new object[] { "a=(a+7)", 5, 12, 12 },
            new object[] { "a=(a+(Int16)7)", 5, 12, 12 },
            new object[] { "a=Int32.MaxValue", 5, Int32.MaxValue, Int32.MaxValue },
        };

        private static object[] IntCompareCases = new object[]
        {
            new object[] { "a < 7", 5, true, 5 },
            new object[] { "a <= 7", 5, true, 5 },
            new object[] { "a == 6", 5, false, 5 },
            new object[] { "a != 6", 5, true, 5 },
            new object[] { "a > 7", 10, true, 10 },
            new object[] { "a >= 7", 5, false, 5 },
            new object[] { "a && 7", 5, true, 5 },
            new object[] { "a || 0", 5, true, 5 },
            new object[] { "true && true", 5, true, 5 },
            new object[] { "true || false", 5, true, 5 },
            new object[] { "a >= 7000000000", 5, false, 5 },
        };

        private static object[] DateTimeCompareCases = new object[]
        {
            new object[] { "new DateTime(50000000000)", new DateTime(50000000000), new DateTime(50000000000), new DateTime(50000000000) },
            new object[] { "a=new DateTime(50000000000)", new DateTime(50000000000), new DateTime(50000000000), new DateTime(50000000000) },
            new object[] { "a.AddTicks((Int64)100)", new DateTime(50000000000), new DateTime(50000000100), new DateTime(50000000000) },
            new object[] { "a=a.AddTicks((Int64)100)", new DateTime(50000000000), new DateTime(50000000100), new DateTime(50000000100) },
        };

        [Test, TestCaseSource("IntCases")]
        public void IntegerTestScenarios(string expression, int initValue, int expectedValue, int expectedVarValue)
        {
            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            context.ExportType(typeof(Int16));
            context.ExportType(typeof(Int32));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile(expression);
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", initValue);
            Func<int> evalFunc = evaluator.Evaluate<int>();
            Assert.AreEqual(evalFunc(), expectedValue);
            Assert.AreEqual(context.Variables()["a"], expectedVarValue);
        }

        [Test, TestCaseSource("IntCompareCases")]
        public void IntegerCompareTestScenarios(string expression, int initValue, bool expectedValue, int expectedVarValue)
        {
            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            context.ExportType(typeof(Int16));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile(expression);
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", initValue);
            Func<bool> evalFunc = evaluator.Evaluate<bool>();
            Assert.AreEqual(evalFunc(), expectedValue);
            Assert.AreEqual(context.Variables()["a"], expectedVarValue);
        }

        [Test, TestCaseSource("DateTimeCompareCases")]
        public void DateTimeTestScenarios(string expression, DateTime initValue, DateTime expectedValue, DateTime expectedVarValue)
        {
            DateTime x = new DateTime();
            Context context = new Context();
            context.DeclareVariable("a", typeof(DateTime));
            context.ExportType(typeof(DateTime));
            context.ExportType(typeof(Int64));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile(expression);
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", initValue);
            Func<DateTime> evalFunc = evaluator.Evaluate<DateTime>();
            Assert.AreEqual(evalFunc(), expectedValue);
            Assert.AreEqual(context.Variables()["a"], expectedVarValue);
        }

        [Test]
        public void OtherAndComplexScenarios()
        {
            string jsonStr = @"{
              'channel': {
                'title': 'Title1',
                'link': 'http://title1.book.com',
                'description': 'Title # 1 book.',
                'item': [
                  {
                    'title': 'Title 1 Chapter 1',
                    'description': 'Chapter 1 of title 1',
                    'link': 'http://title1.book.com/chapter1',
                    'categories': [
                      'tag1',
                      'tag2'
                    ]
                  },
                  {
                    'title': 'Title 1 Chapter 2',
                    'description': 'Chapter 2 of Title 1',
                    'link': 'http://title1.book.com/chapter1',
                    'categories': [
                      'tag3',
                      'tag4'
                    ]
                  }
                ]
              }
            }";

            JObject json = JObject.Parse(jsonStr);

            Context context = new Context();
            context.DeclareVariable("obj", typeof(JObject));
            context.DeclareVariable("suffix", typeof(string));
            context.ExportType(typeof(String));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile("String.Concat(obj[\"channel\"][\"item\"][0][\"title\"].ToString().Substring(0,8), suffix)");
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("obj", json).SetVariable("suffix", "Custom");
            Func<string> evalFunc1 = evaluator.Evaluate<string>();
            Assert.AreEqual(evalFunc1(), "Title 1 Custom");

            context = new Context();
            context.DeclareVariable("a", typeof(int));
            context.ExportType(typeof(Math));
            context.ExportType(typeof(Int32));
            cExp = new ExpressionCompiler(context).Compile("Int32.Parse(((Int32)Math.Sqrt(a * a * 1.0)).ToString()) == 5");
            evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5);
            Func<bool> evalFunc2 = evaluator.Evaluate<bool>();
            Assert.AreEqual(evalFunc2(), true);
        }
    }
}
