using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PrefixUnaryExpressionTransformer : Transformer<Expression, PrefixUnaryExpressionSyntax>
    {
        public static PrefixUnaryExpressionTransformer INSTANCE = new PrefixUnaryExpressionTransformer();

        private readonly MethodInfo tryUpdateMethod;

        public PrefixUnaryExpressionTransformer()
        {
            tryUpdateMethod = typeof(ConcurrentDictionary<string, object>).GetMethod("TryUpdate");
        }

        public Expression ToExpression(Context context, PrefixUnaryExpressionSyntax prefixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, prefixSyntax.Operand);

            switch (prefixSyntax.Kind())
            {
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                    string varName = null;
                    IndexExpression dictionaryAccess = null;
                    if (exp.NodeType == ExpressionType.Convert)
                    {
                        dictionaryAccess = (IndexExpression)((UnaryExpression)exp).Operand;
                        varName = ((ConstantExpression)(dictionaryAccess.Arguments[0])).Value.ToString();
                    }
                    if (varName == null)
                    {
                        throw new CompilationException("Can't perform prefix inc/dec assignment" + prefixSyntax.Operand);
                    }
                    int delta = prefixSyntax.Kind() == SyntaxKind.PreDecrementExpression ? -1 : 1;
                    Expression lResult = Expression.Add(exp, Expression.Constant(delta));
                    Expression lResultObj = Expression.Convert(lResult, typeof(object));
                    return Expression.Block(
                        Expression.Call(Expression.Constant(context.Variables()), tryUpdateMethod, Expression.Constant(varName), lResultObj, dictionaryAccess),
                        exp);
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
