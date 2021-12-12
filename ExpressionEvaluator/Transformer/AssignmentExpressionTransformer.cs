using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class AssignmentExpressionTransformer : Transformer<Expression, AssignmentExpressionSyntax>
    {
        public static AssignmentExpressionTransformer INSTANCE = new AssignmentExpressionTransformer();

        private readonly MethodInfo tryUpdateMethod;

        public AssignmentExpressionTransformer()
        {
            tryUpdateMethod = typeof(ConcurrentDictionary<string, object>).GetMethod("TryUpdate");
        }
        

        public Expression ToExpression(Context context, AssignmentExpressionSyntax assignmentSyntax)
        {
            Expression lExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Left);
            Expression rExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Right);

            string varName = null;
            IndexExpression dictionaryAccess = null;
            if (lExp.NodeType == ExpressionType.Convert)
            {
                dictionaryAccess = (IndexExpression)((UnaryExpression)lExp).Operand;
                varName = ((ConstantExpression)(dictionaryAccess.Arguments[0])).Value.ToString();
            }
            if (varName == null)
            {
                throw new CompilationException("Can't perform assignment ");
            }
            Expression lResult;
            switch (assignmentSyntax.Kind())
            {
                case SyntaxKind.AddAssignmentExpression:
                    lResult = Expression.Add(lExp, rExp);
                    break;
                case SyntaxKind.SubtractAssignmentExpression:
                    lResult = Expression.Subtract(lExp, rExp);
                    break;
                case SyntaxKind.MultiplyAssignmentExpression:
                    lResult = Expression.Multiply(lExp, rExp);
                    break;
                case SyntaxKind.DivideAssignmentExpression:
                    lResult = Expression.Divide(lExp, rExp);
                    break;
                case SyntaxKind.ModuloAssignmentExpression:
                    lResult = Expression.Modulo(lExp, rExp);
                    break;
                case SyntaxKind.AndAssignmentExpression:
                    lResult = Expression.And(lExp, rExp);
                    break;
                case SyntaxKind.OrAssignmentExpression:
                    lResult = Expression.Or(lExp, rExp);
                    break;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    lResult = Expression.ExclusiveOr(lExp, rExp);
                    break;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    lResult = Expression.LeftShift(lExp, rExp);
                    break;
                case SyntaxKind.RightShiftAssignmentExpression:
                    lResult = Expression.RightShift(lExp, rExp);
                    break;
                default:
                    lResult = rExp;
                    break;
            }
            Expression lResultObj = Expression.Convert(lResult, typeof(object));
            return Expression.Block(
                Expression.Call(Expression.Constant(context.Variables()), tryUpdateMethod, Expression.Constant(varName), lResultObj, dictionaryAccess),
                lExp);
        }
    }
}