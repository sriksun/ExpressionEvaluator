using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class ElementAccessExpressionTransformer : Transformer<Expression, ElementAccessExpressionSyntax>
    {
        public static ElementAccessExpressionTransformer INSTANCE = new ElementAccessExpressionTransformer();

        public Expression ToExpression(Context context, ElementAccessExpressionSyntax elementAccessSyntax)
        {
            Expression argExpression = ExpressionFactory.ToExpression(context, elementAccessSyntax.ArgumentList.Arguments[0].Expression);
            Expression exp = ExpressionFactory.ToExpression(context, elementAccessSyntax.Expression);
            PropertyInfo propInfo = exp.Type.GetProperty("Item", new Type[] { argExpression.Type });
            if (propInfo != null)
            {
                try
                {
                    IndexExpression expression = Expression.Property(exp, propInfo, argExpression);
                    return expression;
                } catch(ArgumentException)
                {
                    try
                    {
                        IndexExpression expression = Expression.Property(exp, propInfo, Expression.Convert(argExpression, typeof(object)));
                        return expression;
                    }
                    catch (Exception)
                    {
                        throw new CompilationException("Element Access error " + elementAccessSyntax.GetText());
                    }
                }
            } else if (exp.Type.IsArray && argExpression.Type == typeof(int))
            {
                BinaryExpression expression = Expression.ArrayIndex(exp, argExpression);
                return expression;
            }
            throw new CompilationException("Element Access error " + elementAccessSyntax.GetText());
        }
    }
}
