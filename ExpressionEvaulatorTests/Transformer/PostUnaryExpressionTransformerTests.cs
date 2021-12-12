using System;
using System.Linq;
using System.Linq.Expressions;
using ExpressionEvaluator;
using ExpressionEvaluator.Transformer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace ExpressionEvaulatorTests.Transformer
{
    public class PostUnaryExpressionTransformerTests
    {
        [Test]
        public void TestPostfix()
        {
            var tree = CSharpSyntaxTree.ParseText("a++", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PostfixUnaryExpressionSyntax postfixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PostfixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            try
            {
                UnaryExpression cExp = PostUnaryExpressionTransformer.INSTANCE.ToExpression(context, postfixSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

    }
}
