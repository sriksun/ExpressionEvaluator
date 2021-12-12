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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionEvaluator;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class ExpressionEvaluatorTests
    {
        [Test]
        public void TestExpressionEvaluator()
        {
            Context context = new Context();
            CompiledExpression cExp = new CompiledExpression(context, Expression.Constant(5));
            ExpressionEvaluator.ExpressionEvaluator evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            Func<int> l = evaluator.Evaluate<int>();
            Assert.AreEqual(l(), 5);

            context.DeclareVariable("a", typeof(int));
            cExp = new CompiledExpression(context, Expression.Constant(5));
            evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5);
            Assert.AreEqual(context.Variables()["a"], 5);

            //Let us define Expr(a+2) as Expression(Variables.Item("a") + 2), where Variables is in Context::variableHolder
            Expression argExpression = Expression.Constant("a");
            PropertyInfo pInfo = typeof(ConcurrentDictionary<string, object>).GetProperty("Item", typeof(object));
            Expression expression = Expression.Convert(Expression.Property(Expression.Constant(context.Variables(),
                typeof(ConcurrentDictionary<string, object>)), pInfo, argExpression), typeof(int));
            expression = Expression.Add(expression, Expression.Constant(2));

            cExp = new CompiledExpression(context, Expression.Block(context.VariableDeclarations(), expression));
            evaluator = new ExpressionEvaluator.ExpressionEvaluator(cExp);
            l = evaluator.Evaluate<int>();
            Assert.AreEqual(l(), 7);
        }
    }
}
