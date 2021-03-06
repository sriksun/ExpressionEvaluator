/*
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements.  See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership.  The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License.  You may obtain a copy of the License at
*
*     https://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    /// <summary>
    /// ExpressionEvaluator evaluates simple expressions in a performant fashion.
    /// Evaluator compiles an expression, binds variables at evaluation time.
    /// <see cref="ExpressionEvaluator.Evaluate{T}"/>
    /// <code>
    ///     Context context = new Context();
    ///     context.DeclareVariable("a", typeof(int));
    ///     CompiledExpression cExp = new ExpressionCompiler(context).Compile("a+=3");
    ///     ExpressionEvaluator evaluator = new ExpressionEvaluator(cExp);
    ///     evaluator.SetVariable("a", 5);
    ///     Func<int> func = evaluator.Evaluate<int>();
    ///     func();
    /// </code>
    /// Note: ExpressionEvalution is not thread-safe.
    /// </summary>
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
