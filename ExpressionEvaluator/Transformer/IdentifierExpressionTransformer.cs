using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class IdentifierExpressionTransformer : Transformer<Expression, IdentifierNameSyntax>
    {
        public static IdentifierExpressionTransformer INSTANCE = new IdentifierExpressionTransformer();

        public Expression ToExpression(Context context, IdentifierNameSyntax identifierSyntax)
        {
            string identifier = identifierSyntax.Identifier.ValueText;
            var param = context.VariableDeclarations().FirstOrDefault(v => v.Name == identifier);

            if (param == null)
            {
                throw new CompilationException("Unknown variable " + identifier);
            }
            Expression argExpression = Expression.Constant(identifier);
            PropertyInfo pInfo = typeof(ConcurrentDictionary<string, object>).GetProperty("Item", typeof(object));
            Expression expression = Expression.Convert(Expression.Property(Expression.Constant(context.Variables(),
                typeof(ConcurrentDictionary<string, object>)), pInfo, argExpression), param.Type);
            return expression;
        }
    }
}
