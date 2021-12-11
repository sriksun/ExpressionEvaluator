using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class CastExpressionTransformer : Transformer<Expression, CastExpressionSyntax>
    {
        public static CastExpressionTransformer INSTANCE = new CastExpressionTransformer();

        public Expression ToExpression(Context context, CastExpressionSyntax castExpSyntax)
        {
            Type type = context.ExportedTypes().FirstOrDefault(v => v.Name == castExpSyntax.Type.ToString());
            if (type == null)
            {
                type = Type.GetType(castExpSyntax.Type.ToString());
            }
            return Expression.Convert(ExpressionFactory.ToExpression(context, castExpSyntax.Expression), type);
        }
    }
}
