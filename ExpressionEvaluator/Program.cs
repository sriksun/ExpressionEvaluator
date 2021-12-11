using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace ExpressionEvaluator
{
    class Program
    {
        static void Main(string[] args)
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
            context.DeclareVariable("a", typeof(int));
            context.DeclareVariable("obj", typeof(JObject));
            context.DeclareVariable("suffix", typeof(string));
            context.ExportType(typeof(Math));
            context.ExportType(typeof(Int32));
            context.ExportType(typeof(String));
            context.ExportType(typeof(BitConverter));
            context.ExportType(typeof(DateTime));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile("String.Concat(obj[\"channel\"][\"item\"][0][\"title\"].ToString().Substring(0,8), suffix)");
            ExpressionEvaluator evaluator = new ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5).SetVariable("obj", json).SetVariable("suffix", "");
            Func<string> v = evaluator.Evaluate<string>();
            Console.WriteLine(v());
            Console.WriteLine(v());

            Stopwatch watch = Stopwatch.StartNew();

            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                evaluator.SetVariable("suffix", i.ToString());
                v();
            }
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Restart();
            for (int i = 0; i < 1000000; i++)
            {
                string s = String.Concat(json["channel"]["item"][0]["title"].ToString().Substring(0, 8), i.ToString());
            }
            Console.WriteLine(watch.ElapsedMilliseconds);

            context = new Context();
            context.DeclareVariable("a", typeof(int));
            context.ExportType(typeof(Math));
            context.ExportType(typeof(Int32));
             cExp = new ExpressionCompiler(context).Compile("Int32.Parse(((Int32)Math.Sqrt(a * a * 1.0)).ToString()) == 5");
            evaluator = new ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5);
            Func<bool> v1 = evaluator.Evaluate<bool>();
            Console.WriteLine(v1());

            context = new Context();
            context.ExportType(typeof(DateTime));
            cExp = new ExpressionCompiler(context).Compile("new DateTime(DateTime.Now.Ticks)");
            evaluator = new ExpressionEvaluator(cExp);
            Func<DateTime> v2 = evaluator.Evaluate<DateTime>();
            Console.WriteLine(v2());

            context = new Context();
            context.ExportType(typeof(DateTime));
            cExp = new ExpressionCompiler(context).Compile("new DateTime(DateTime.Now.Ticks)");
            evaluator = new ExpressionEvaluator(cExp);
            Func<DateTime> v3 = evaluator.Evaluate<DateTime>();
            Console.WriteLine(v3());
        }
    }
}
