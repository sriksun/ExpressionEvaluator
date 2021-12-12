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
    public class ObjCreationExpressionTransformerTests
    {
        [Test]
        public void TestUnexportTypeObjCreation()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime()", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestObjCreationWrongConstructor()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime(false)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            context.ExportType(typeof(DateTime));
            try
            {
                NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestObjCreation()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime(50000000000)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            context.ExportType(typeof(DateTime));
            NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.New);
            Assert.AreEqual(cExp.Type, typeof(DateTime));
        }
    }
}
