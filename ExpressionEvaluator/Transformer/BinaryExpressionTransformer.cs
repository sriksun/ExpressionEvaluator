using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class BinaryExpressionTransformer : Transformer<BinaryExpression, BinaryExpressionSyntax>
    {
        public static BinaryExpressionTransformer INSTANCE = new BinaryExpressionTransformer();

        private Dictionary<Type, int> priority = new Dictionary<Type, int>()
        {
            { typeof(byte), 1 },
            { typeof(short), 2 },
            { typeof(int), 3 },
            { typeof(long), 4 },
            { typeof(float), 5 },
            { typeof(double), 6 }
        };

        public BinaryExpression ToExpression(Context context, BinaryExpressionSyntax binarySyntax)
        {
            Expression lExp = ExpressionFactory.ToExpression(context, binarySyntax.Left);
            Expression rExp = ExpressionFactory.ToExpression(context, binarySyntax.Right);
            int lTypePrio = priority.ContainsKey(lExp.Type) ? priority[lExp.Type] : 0;
            int rTypePrio = priority.ContainsKey(rExp.Type) ? priority[rExp.Type] : 0;

            if (lTypePrio > 0 && rTypePrio > 0 & lTypePrio != rTypePrio)
            {
                if (lTypePrio < rTypePrio)
                {
                    lExp = Expression.Convert(lExp, rExp.Type);
                }
                else
                {
                    rExp = Expression.Convert(rExp, lExp.Type);
                }
            }

            switch (binarySyntax.Kind())
            {
                case SyntaxKind.AddExpression:
                    return Expression.Add(lExp, rExp);
                case SyntaxKind.SubtractExpression:
                    return Expression.Subtract(lExp, rExp);
                case SyntaxKind.MultiplyExpression:
                    return Expression.Multiply(lExp, rExp);
                case SyntaxKind.DivideExpression:
                    return Expression.Divide(lExp, rExp);
                case SyntaxKind.ModuloExpression:
                    return Expression.Modulo(lExp, rExp);
                case SyntaxKind.EqualsExpression:
                    return Expression.Equal(lExp, rExp);
                case SyntaxKind.NotEqualsExpression:
                    return Expression.NotEqual(lExp, rExp);
                case SyntaxKind.LessThanExpression:
                    return Expression.LessThan(lExp, rExp);
                case SyntaxKind.LessThanOrEqualExpression:
                    return Expression.LessThanOrEqual(lExp, rExp);
                case SyntaxKind.GreaterThanExpression:
                    return Expression.GreaterThan(lExp, rExp);
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return Expression.GreaterThanOrEqual(lExp, rExp);
                case SyntaxKind.LogicalAndExpression:
                    Expression rhs0 = ((lTypePrio + rTypePrio) > 0 ? Expression.Constant(0) : Expression.Constant(true));
                    return Expression.NotEqual(Expression.And(lExp, rExp), rhs0);
                case SyntaxKind.LogicalOrExpression:
                    Expression rhs1 = ((lTypePrio + rTypePrio) > 0 ? Expression.Constant(0) : Expression.Constant(true));
                    return Expression.NotEqual(Expression.Or(lExp, rExp), rhs1);
                case SyntaxKind.CoalesceExpression:
                    return Expression.Coalesce(lExp, rExp);
                case SyntaxKind.BitwiseAndExpression:
                    return Expression.And(lExp, rExp);
                case SyntaxKind.BitwiseOrExpression:
                    return Expression.Or(lExp, rExp);
                case SyntaxKind.ExclusiveOrExpression:
                    return Expression.ExclusiveOr(lExp, rExp);
                case SyntaxKind.LeftShiftExpression:
                    return Expression.LeftShift(lExp, rExp);
                case SyntaxKind.RightShiftExpression:
                    return Expression.RightShift(lExp, rExp);
                default:
                    throw new CompilationException("Unknown binary expression" + binarySyntax.GetText());
            }
        }
    }
}
