using System;
using System.Diagnostics;
using ExpressionEvaluator;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class PerformanceTests
    {

        [Test]
        public void PerformanceBenchmark()
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

            Stopwatch watch = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                evaluator.SetVariable("suffix", i.ToString());
                evalFunc1();
            }
            long expTime = watch.ElapsedMilliseconds;
            watch.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                string s = string.Concat(json["channel"]["item"][0]["title"].ToString().Substring(0, 8), i.ToString());
            }
            long nativeTime = watch.ElapsedMilliseconds;
            Assert.Less(expTime, nativeTime * 3);
        }
    }
}
