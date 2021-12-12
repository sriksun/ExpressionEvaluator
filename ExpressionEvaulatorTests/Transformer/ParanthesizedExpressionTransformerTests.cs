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
    public class ParanthesizedExpressionTransformerTests
    {
        [Test]
        public void TestParanthesis()
        {
            var tree = CSharpSyntaxTree.ParseText("(5)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ParenthesizedExpressionSyntax parenthesizedSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ParenthesizedExpressionSyntax>()
                .First();

            Context context = new Context(); 
            Expression cExp = ParenthesizedExpressionTransformer.INSTANCE.ToExpression(context, parenthesizedSyntax);
            Assert.AreEqual(((ConstantExpression)cExp).Value, 5);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

    }
}
