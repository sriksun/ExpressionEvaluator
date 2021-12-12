using System.Linq.Expressions;
using ExpressionEvaluator.Transformer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator
{
    public class ExpressionFactory
    {
        public static Expression ToExpression<B>(Context context, B expressionSyntax) where B : ExpressionSyntax
        {
            switch(expressionSyntax)
            {
                case LiteralExpressionSyntax literalSyntax:
                    return LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
                case AssignmentExpressionSyntax assignmentSyntax:
                    return AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
                case BinaryExpressionSyntax binarySyntax:
                    return BinaryExpressionTransformer.INSTANCE.ToExpression(context, binarySyntax);
                case InvocationExpressionSyntax invocationSyntax:
                    return InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
                case IdentifierNameSyntax identifierSyntax:
                    return IdentifierExpressionTransformer.INSTANCE.ToExpression(context, identifierSyntax);
                case ElementAccessExpressionSyntax elementAccessSyntax:
                    return ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
                case ObjectCreationExpressionSyntax objCreationSyntax:
                    return ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
                case MemberAccessExpressionSyntax memberAccessSyntax:
                    return MemberAccessExpressionTransformer.INSTANCE.ToExpression(context, memberAccessSyntax);
                case ParenthesizedExpressionSyntax parenthesizedSyntax:
                    return ParenthesizedExpressionTransformer.INSTANCE.ToExpression(context, parenthesizedSyntax);
                case CastExpressionSyntax castSyntax:
                    return CastExpressionTransformer.INSTANCE.ToExpression(context, castSyntax);
                case PrefixUnaryExpressionSyntax prefixSyntax:
                    return PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
                case PostfixUnaryExpressionSyntax postfixSyntax:
                    return PostUnaryExpressionTransformer.INSTANCE.ToExpression(context, postfixSyntax);
                default:
                    throw new CompilationException("Unsupported expression syntax " + expressionSyntax.GetText());
            }
        }
    }
}
