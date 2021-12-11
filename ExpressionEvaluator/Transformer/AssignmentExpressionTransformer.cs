using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class AssignmentExpressionTransformer : Transformer<BinaryExpression, AssignmentExpressionSyntax>
    {
        public static AssignmentExpressionTransformer INSTANCE = new AssignmentExpressionTransformer();

        public BinaryExpression ToExpression(Context context, AssignmentExpressionSyntax assignmentSyntax)
        {
            Expression lExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Left);
            Expression rExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Right);

            switch (assignmentSyntax.Kind())
            {
                case SyntaxKind.AddAssignmentExpression:
                    return Expression.AddAssign(lExp, rExp);
                case SyntaxKind.SubtractAssignmentExpression:
                    return Expression.SubtractAssign(lExp, rExp);
                case SyntaxKind.MultiplyAssignmentExpression:
                    return Expression.MultiplyAssign(lExp, rExp);
                case SyntaxKind.DivideAssignmentExpression:
                    return Expression.DivideAssign(lExp, rExp);
                case SyntaxKind.ModuloAssignmentExpression:
                    return Expression.ModuloAssign(lExp, rExp);
                case SyntaxKind.AndAssignmentExpression:
                    return Expression.AndAssign(lExp, rExp);
                case SyntaxKind.OrAssignmentExpression:
                    return Expression.OrAssign(lExp, rExp);
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    return Expression.ExclusiveOrAssign(lExp, rExp);
                case SyntaxKind.LeftShiftAssignmentExpression:
                    return Expression.LeftShiftAssign(lExp, rExp);
                case SyntaxKind.RightShiftAssignmentExpression:
                    return Expression.RightShiftAssign(lExp, rExp);
                default:
                    return Expression.Assign(lExp, rExp);
            }
        }
    }
}