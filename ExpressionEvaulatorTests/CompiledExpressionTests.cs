using System.Linq.Expressions;
using ExpressionEvaluator;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class CompiledExpressionTests
    {
        [Test]
        public void TestCompiledExpression()
        {
            Context context = new Context();
            ConstantExpression exp = Expression.Constant(5);
            CompiledExpression cExp = new CompiledExpression(context, exp);
            Assert.AreEqual(context, cExp.Context());
            Assert.AreEqual(exp, cExp.Expression());
        }
    }
}
