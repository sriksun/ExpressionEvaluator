using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PrefixUnaryExpressionTransformer : Transformer<UnaryExpression, PrefixUnaryExpressionSyntax>
    {
        public static PrefixUnaryExpressionTransformer INSTANCE = new PrefixUnaryExpressionTransformer();

        public UnaryExpression ToExpression(Context context, PrefixUnaryExpressionSyntax prefixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, prefixSyntax.Operand);

            switch (prefixSyntax.Kind())
            {
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                    throw new CompilationException("Prefix Increment/Decrement not supported" + prefixSyntax.Operand);
                case SyntaxKind.BitwiseNotExpression:
                    return Expression.Not(exp);
                case SyntaxKind.LogicalNotExpression:
                    return Expression.Not(exp);
                case SyntaxKind.UnaryMinusExpression:
                    return Expression.Negate(exp);
                default:
                    throw new CompilationException("Unable to resolve unary expression" + prefixSyntax.Operand);
            }

        }
    }
}
