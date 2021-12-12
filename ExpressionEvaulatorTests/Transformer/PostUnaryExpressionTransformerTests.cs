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
        public void TestPostfixDecrement()
        {
            var tree = CSharpSyntaxTree.ParseText("a--", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PostfixUnaryExpressionSyntax postfixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PostfixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = PostUnaryExpressionTransformer.INSTANCE.ToExpression(context, postfixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPostfixIncrement()
        {
            var tree = CSharpSyntaxTree.ParseText("a++", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PostfixUnaryExpressionSyntax postfixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PostfixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = PostUnaryExpressionTransformer.INSTANCE.ToExpression(context, postfixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }
    }
}
