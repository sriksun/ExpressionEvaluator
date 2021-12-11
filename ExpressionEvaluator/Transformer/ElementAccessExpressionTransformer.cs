using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class ElementAccessExpressionTransformer : Transformer<IndexExpression, ElementAccessExpressionSyntax>
    {
        public static ElementAccessExpressionTransformer INSTANCE = new ElementAccessExpressionTransformer();

        public IndexExpression ToExpression(Context context, ElementAccessExpressionSyntax elementAccessSyntax)
        {
            Expression argExpression = ExpressionFactory.ToExpression(context, elementAccessSyntax.ArgumentList.Arguments[0].Expression);
            Expression exp = ExpressionFactory.ToExpression(context, elementAccessSyntax.Expression);
            PropertyInfo propInfo = exp.Type.GetProperty("Item", new Type[] { typeof(object) });
            if (propInfo != null)
            {
                IndexExpression expression = Expression.Property(exp, propInfo, Expression.Convert(argExpression, typeof(object)));
                return expression;
            } else
            {
                throw new CompilationException("Element Access error" + elementAccessSyntax.GetText());
            }
        }
    }
}
