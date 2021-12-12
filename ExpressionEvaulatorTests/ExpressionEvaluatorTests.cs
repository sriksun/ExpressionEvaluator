using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionEvaluator;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class ExpressionEvaluatorTests
    {
        [Test]
        public void TestExpressionEvaluator()
        {
            Context context = new Context();
            CompiledExpression cExp = new CompiledExpression(context, Expression.Constant(5));
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            Func<int> l = evaluator.Evaluate<int>();
            Assert.AreEqual(l(), 5);

            context.DeclareVariable("a", typeof(int));
            cExp = new CompiledExpression(context, Expression.Constant(5));
            evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5);
            Assert.AreEqual(context.Variables()["a"], 5);

            //Let us define Expr(a+2) as Expression(Variables.Item("a") + 2), where Variables is in Context::variableHolder
            Expression argExpression = Expression.Constant("a");
            PropertyInfo pInfo = typeof(ConcurrentDictionary<string, object>).GetProperty("Item", typeof(object));
            Expression expression = Expression.Convert(Expression.Property(Expression.Constant(context.Variables(),
                typeof(ConcurrentDictionary<string, object>)), pInfo, argExpression), typeof(int));
            expression = Expression.Add(expression, Expression.Constant(2));

            cExp = new CompiledExpression(context, Expression.Block(context.VariableDeclarations(), expression));
            evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            l = evaluator.Evaluate<int>();
            Assert.AreEqual(l(), 7);
        }
    }
}
