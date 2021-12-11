using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PostUnaryExpressionTransformer : Transformer<UnaryExpression, PostfixUnaryExpressionSyntax>
    {
        public static PostUnaryExpressionTransformer INSTANCE = new PostUnaryExpressionTransformer();

        public UnaryExpression ToExpression(Context context, PostfixUnaryExpressionSyntax prefixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, prefixSyntax.Operand);

            switch (prefixSyntax.Kind())
            {
                case SyntaxKind.PostDecrementExpression:
                    return Expression.PostDecrementAssign(exp);
                case SyntaxKind.PostIncrementExpression:
                    return Expression.PostIncrementAssign(exp);
                default:
                    throw new CompilationException("Unable to resolve unary expression" + prefixSyntax.Operand);
            }

        }
    }
}
