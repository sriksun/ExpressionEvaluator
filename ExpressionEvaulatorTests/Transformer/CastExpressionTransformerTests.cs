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
    public class CastExpressionTransformerTests
    {
        [Test]
        public void TestCast()
        {
            var tree = CSharpSyntaxTree.ParseText("(Int64)5", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            CastExpressionSyntax castSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<CastExpressionSyntax>()
                .First();

            Context context = new Context();
            context.ExportType(typeof(Int64));
            Expression cExp = CastExpressionTransformer.INSTANCE.ToExpression(context, castSyntax);
            Assert.AreEqual(cExp.Type, typeof(Int64));
        }

    }
}
