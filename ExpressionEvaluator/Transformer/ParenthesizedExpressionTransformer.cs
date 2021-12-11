using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class ParenthesizedExpressionTransformer : Transformer<Expression, ParenthesizedExpressionSyntax>
    {
        public static ParenthesizedExpressionTransformer INSTANCE = new ParenthesizedExpressionTransformer();

        public Expression ToExpression(Context context, ParenthesizedExpressionSyntax parenthesizedSyntax)
        {
            return ExpressionFactory.ToExpression(context, parenthesizedSyntax.Expression);
        }
    }
}
