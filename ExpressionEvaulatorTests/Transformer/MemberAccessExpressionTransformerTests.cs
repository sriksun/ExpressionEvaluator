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
    public class MemberAccessExpressionTransformerTests
    {
        [Test]
        public void TestMemberAccess()
        {
            var tree = CSharpSyntaxTree.ParseText("a.Length", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            MemberAccessExpressionSyntax memberAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("a", typeof(string));
            MemberExpression cExp = MemberAccessExpressionTransformer.INSTANCE.ToExpression(context, memberAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestMemberAccessInvalidProperty()
        {
            var tree = CSharpSyntaxTree.ParseText("a.LengthA", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            MemberAccessExpressionSyntax memberAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("a", typeof(string));
            try
            {
                MemberExpression cExp = MemberAccessExpressionTransformer.INSTANCE.ToExpression(context, memberAccessSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestMemberAccessStaticMemberAccess()
        {
            var tree = CSharpSyntaxTree.ParseText("DateTime.Now", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            MemberAccessExpressionSyntax memberAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.ExportType(typeof(DateTime));
            MemberExpression cExp = MemberAccessExpressionTransformer.INSTANCE.ToExpression(context, memberAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(DateTime));
        }

        [Test]
        public void TestMemberAccessStaticMemberAccessError()
        {
            var tree = CSharpSyntaxTree.ParseText("DateTime.Now1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            MemberAccessExpressionSyntax memberAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.ExportType(typeof(DateTime));
            try
            {
                MemberExpression cExp = MemberAccessExpressionTransformer.INSTANCE.ToExpression(context, memberAccessSyntax);
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
