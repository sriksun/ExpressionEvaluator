using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public class CompiledExpression
    {
        private readonly Context context;
        private readonly Expression expression;

        public CompiledExpression(Context context, Expression expression)
        {
            this.context = context;
            this.expression = expression;
        }

        public Expression Expression()
        {
            return expression;
        }

        public Context Context()
        {
            return context;
        }
    }
}
