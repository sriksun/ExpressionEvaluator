using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator
{
    public class ExpressionCompiler
    {
        private readonly Context context;

        public ExpressionCompiler(Context context) 
        {
            this.context = context;
        }

        private SyntaxTree ValidateExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
                throw new Exception("Expression cannot be empty or null");

            var tree = CSharpSyntaxTree.ParseText(expression, CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            var diagnostics = tree.GetDiagnostics().ToList();

            if (diagnostics.Count > 0)
                throw new Exception(diagnostics[0].ToString());

            return tree;
        }

        public CompiledExpression Compile(string expression)
        {
            ExpressionStatementSyntax expr = ValidateExpression(expression)
                .GetRoot()
                .DescendantNodes()
                .OfType<ExpressionStatementSyntax>()
                .First();

            Expression exp = ExpressionFactory.ToExpression(context, expr.ChildNodes().OfType<ExpressionSyntax>().First());

            return new CompiledExpression(context, Expression.Block(
                context.VariableDeclarations(),
                exp
            ));
        }
    }
}
