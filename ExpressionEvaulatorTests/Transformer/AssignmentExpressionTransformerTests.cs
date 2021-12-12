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
    public class AssignmentExpressionTransformerTests
    {
        [Test]
        public void TestAdd()
        {
            var tree = CSharpSyntaxTree.ParseText("a+=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestAssign()
        {
            var tree = CSharpSyntaxTree.ParseText("a=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestAddError()
        {
            var tree = CSharpSyntaxTree.ParseText("5+=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            } catch(CompilationException)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestSubtract()
        {
            var tree = CSharpSyntaxTree.ParseText("a-=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestMultiply()
        {
            var tree = CSharpSyntaxTree.ParseText("a*=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestDivide()
        {
            var tree = CSharpSyntaxTree.ParseText("a/=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestModulo()
        {
            var tree = CSharpSyntaxTree.ParseText("a%=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestLeftShift()
        {
            var tree = CSharpSyntaxTree.ParseText("a <<= 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestRightShift()
        {
            var tree = CSharpSyntaxTree.ParseText("a >>= 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseAnd()
        {
            var tree = CSharpSyntaxTree.ParseText("a &= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseOr()
        {
            var tree = CSharpSyntaxTree.ParseText("a |= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseXOr()
        {
            var tree = CSharpSyntaxTree.ParseText("a ^= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }
    }
}
