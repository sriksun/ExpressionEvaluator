using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public interface Transformer<A, B> where B : ExpressionSyntax where A : Expression
    {
        A ToExpression(Context context, B expressionSyntax) ;
    }
}
