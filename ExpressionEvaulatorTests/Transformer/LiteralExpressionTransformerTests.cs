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
    public class LiteralExpressionTransformerTests
    {
        [Test]
        public void TestNumericLiteralInt()
        {
            var tree = CSharpSyntaxTree.ParseText("5", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context(); 
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 5);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestNumericLiteralDouble()
        {
            var tree = CSharpSyntaxTree.ParseText("5.1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 5.1);
            Assert.AreEqual(cExp.Type, typeof(double));
        }

        [Test]
        public void TestNumericLiteralException()
        {
            var tree = CSharpSyntaxTree.ParseText("5dhfg", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            } catch (Exception ignore)
            {
                Assert.AreEqual(ignore.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestLiteralString()
        {
            var tree = CSharpSyntaxTree.ParseText("\"hello\"", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, "hello");
            Assert.AreEqual(cExp.Type, typeof(string));
        }

        [Test]
        public void TestLiteralChar()
        {
            var tree = CSharpSyntaxTree.ParseText("'h'", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 'h');
            Assert.AreEqual(cExp.Type, typeof(char));
        }

        [Test]
        public void TestLiteralTueBool()
        {
            var tree = CSharpSyntaxTree.ParseText("true", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, true);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLiteralFalseBool()
        {
            var tree = CSharpSyntaxTree.ParseText("false", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, false);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLiteralNull()
        {
            var tree = CSharpSyntaxTree.ParseText("null", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, null);
            Assert.AreEqual(cExp.Type, typeof(object));
        }

    }
}
