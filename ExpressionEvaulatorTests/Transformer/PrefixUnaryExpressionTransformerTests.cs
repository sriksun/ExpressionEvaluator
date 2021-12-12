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
    public class PrefixUnaryExpressionTransformerTests
    {
        [Test]
        public void TestPrefix()
        {
            var tree = CSharpSyntaxTree.ParseText("--a", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            try
            {
                UnaryExpression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestPrefixNegate()
        {
            var tree = CSharpSyntaxTree.ParseText("-5", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Negate);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, 5);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixNot()
        {
            var tree = CSharpSyntaxTree.ParseText("~7", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Not);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, 7);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixLogicalNot()
        {
            var tree = CSharpSyntaxTree.ParseText("!true", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Not);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, true);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }
    }
}
