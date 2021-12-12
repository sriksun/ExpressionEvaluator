using System;
using System.Collections.Generic;
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
    public class ElementAccessExpressionTransformerTests
    {
        [Test]
        public void TestElementAccessDictionary()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(Dictionary<int, int>));
            Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestElementAccessArray()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(int[]));
            Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestElementAccessInvalid()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(string));
            try
            {
                Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
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
