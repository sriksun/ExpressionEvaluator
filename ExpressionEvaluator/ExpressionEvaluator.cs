using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public class ExpressionEvaluator
    {
        private readonly Context context;
        private readonly Expression expression;

        public ExpressionEvaluator(CompiledExpression cExp)
        {
            this.context = cExp.Context();
            this.expression = cExp.Expression();
        }

        public ExpressionEvaluator SetVariable(string name, object val)
        {
            context.SetVariable(name, val);
            return this;
        }

        public ExpressionEvaluator SetVariables(Dictionary<string, object> variables)
        {
            context.SetVariables(variables);
            return this;
        }

        private T EvaluateImpl<T>()
        {
            Expression<T> exp = Expression.Lambda<T>(expression);
            T val = exp.Compile();
            return val;
        }

        public Func<T> Evaluate<T>() => EvaluateImpl<Func<T>>();
    }
}
