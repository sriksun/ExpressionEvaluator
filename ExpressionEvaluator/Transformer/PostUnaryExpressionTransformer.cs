using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PostUnaryExpressionTransformer : Transformer<UnaryExpression, PostfixUnaryExpressionSyntax>
    {
        public static PostUnaryExpressionTransformer INSTANCE = new PostUnaryExpressionTransformer();

        public UnaryExpression ToExpression(Context context, PostfixUnaryExpressionSyntax postfixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, postfixSyntax.Operand);

            switch (postfixSyntax.Kind())
            {
                case SyntaxKind.PostDecrementExpression:
                case SyntaxKind.PostIncrementExpression:
                    throw new CompilationException("Postfix Increment/Decrement not supported" + postfixSyntax.Operand);
                default:
                    throw new CompilationException("Unable to resolve unary expression" + postfixSyntax.Operand);
            }

        }
    }
}
