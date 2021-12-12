using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class LiteralExpressionTransformer : Transformer<ConstantExpression, LiteralExpressionSyntax>
    {
        public static LiteralExpressionTransformer INSTANCE = new LiteralExpressionTransformer();

        public ConstantExpression ToExpression(Context context, LiteralExpressionSyntax literalSyntax)
        {
            string literal = literalSyntax.GetText().ToString();
            switch (literalSyntax.Kind())
            {
                case SyntaxKind.NumericLiteralExpression:
                    int intVal;
                    if (!int.TryParse(literal, out intVal))
                    {
                        long longVal;
                        if (!long.TryParse(literal, out longVal))
                        {
                            double doubleVal;
                            if (!double.TryParse(literal, out doubleVal))
                            {
                                throw new CompilationException("Unable to parse Numeric literal " + literal);
                            }
                            return Expression.Constant(doubleVal, typeof(double));

                        }
                        return Expression.Constant(longVal, typeof(long));
                    }
                    return Expression.Constant(intVal, typeof(int));
                case SyntaxKind.StringLiteralExpression:
                    return Expression.Constant(literal.TrimStart('"').TrimEnd('"'), typeof(string));
                case SyntaxKind.CharacterLiteralExpression:
                    return Expression.Constant(literal.TrimStart('\'').TrimEnd('\'')[0], typeof(char));
                case SyntaxKind.TrueLiteralExpression:
                    return Expression.Constant(true, typeof(bool));
                case SyntaxKind.FalseLiteralExpression:
                    return Expression.Constant(false, typeof(bool));
                case SyntaxKind.NullLiteralExpression:
                    return Expression.Constant(null);
                default:
                    throw new CompilationException("Unknown literal " + literal); ;
            }
        }
    }
}
