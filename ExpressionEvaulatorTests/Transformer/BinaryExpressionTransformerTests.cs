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
    public class BinaryExpressionTransformerTests
    {
        [Test]
        public void TestAdd()
        {
            var tree = CSharpSyntaxTree.ParseText("5+3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Add);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestSubtract()
        {
            var tree = CSharpSyntaxTree.ParseText("5-3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Subtract);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestMultiply()
        {
            var tree = CSharpSyntaxTree.ParseText("5*3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Multiply);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestDivide()
        {
            var tree = CSharpSyntaxTree.ParseText("5/3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Divide);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestModulo()
        {
            var tree = CSharpSyntaxTree.ParseText("5%3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Modulo);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestLeftShift()
        {
            var tree = CSharpSyntaxTree.ParseText("5 << 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.LeftShift);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestRightShift()
        {
            var tree = CSharpSyntaxTree.ParseText("5 >> 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.RightShift);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestCoalesce()
        {
            var tree = CSharpSyntaxTree.ParseText("a ?? 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int?));
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Coalesce);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestLessthan()
        {
            var tree = CSharpSyntaxTree.ParseText("5 < 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.LessThan);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLessthanEqual()
        {
            var tree = CSharpSyntaxTree.ParseText("5 <= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.LessThanOrEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestGreaterthan()
        {
            var tree = CSharpSyntaxTree.ParseText("5 > 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.GreaterThan);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestGreaterthanEqual()
        {
            var tree = CSharpSyntaxTree.ParseText("5 >= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.GreaterThanOrEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestEqual()
        {
            var tree = CSharpSyntaxTree.ParseText("5 == 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Equal);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestNotEqual()
        {
            var tree = CSharpSyntaxTree.ParseText("5 != 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.NotEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLogicalAndNumeric()
        {
            var tree = CSharpSyntaxTree.ParseText("5 && 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.NotEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLogicalOrNumeric()
        {
            var tree = CSharpSyntaxTree.ParseText("5 || 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.NotEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLogicalAndBool()
        {
            var tree = CSharpSyntaxTree.ParseText("true && true", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.NotEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLogicalOrBool()
        {
            var tree = CSharpSyntaxTree.ParseText("true || false", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.NotEqual);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestBitwiseAnd()
        {
            var tree = CSharpSyntaxTree.ParseText("5 & 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.And);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseOr()
        {
            var tree = CSharpSyntaxTree.ParseText("5 | 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Or);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseXOr()
        {
            var tree = CSharpSyntaxTree.ParseText("5 ^ 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            BinaryExpressionSyntax binarySyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .First();

            Context context = new Context();
            BinaryExpression cExp = BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.ExclusiveOr);
            Assert.AreEqual(cExp.Type, typeof(int));
        }
    }
}
