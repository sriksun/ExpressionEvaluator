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
    public class IdentifierExpressionTransformerTests
    {
        [Test]
        public void TestValidVariable()
        {
            var tree = CSharpSyntaxTree.ParseText("a", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            IdentifierNameSyntax identifierSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(DateTime));
            Expression cExp = IdentifierExpressionTransformer.INSTANCE.ToExpression(context, identifierSyntax);
            Assert.AreEqual(cExp.Type, typeof(DateTime));
        }

        [Test]
        public void TestInvalidVariable()
        {
            var tree = CSharpSyntaxTree.ParseText("b", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            IdentifierNameSyntax identifierSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(DateTime));
            try
            {
                Expression cExp = IdentifierExpressionTransformer.INSTANCE.ToExpression(context, identifierSyntax);
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
