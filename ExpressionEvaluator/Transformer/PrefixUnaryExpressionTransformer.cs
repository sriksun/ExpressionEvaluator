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

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PrefixUnaryExpressionTransformer : IExpressionTransformer<Expression, PrefixUnaryExpressionSyntax>
    {
        public static PrefixUnaryExpressionTransformer INSTANCE = new PrefixUnaryExpressionTransformer();

        private readonly MethodInfo tryUpdateMethod;

        public PrefixUnaryExpressionTransformer()
        {
            tryUpdateMethod = typeof(ConcurrentDictionary<string, object>).GetMethod("TryUpdate");
        }

        public Expression ToExpression(Context context, PrefixUnaryExpressionSyntax prefixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, prefixSyntax.Operand);

            switch (prefixSyntax.Kind())
            {
                case SyntaxKind.PreIncrementExpression:
                case SyntaxKind.PreDecrementExpression:
                    string varName = null;
                    IndexExpression dictionaryAccess = null;
                    if (exp.NodeType == ExpressionType.Convert)
                    {
                        dictionaryAccess = (IndexExpression)((UnaryExpression)exp).Operand;
                        varName = ((ConstantExpression)(dictionaryAccess.Arguments[0])).Value.ToString();
                    }
                    if (varName == null)
                    {
                        throw new CompilationException("Can't perform prefix inc/dec assignment" + prefixSyntax.Operand);
                    }
                    int delta = prefixSyntax.Kind() == SyntaxKind.PreDecrementExpression ? -1 : 1;
                    Expression lResult = Expression.Add(exp, Expression.Constant(delta));
                    Expression lResultObj = Expression.Convert(lResult, typeof(object));
                    //Assignment to variable is achieved through update to concurrent dictionary holding all the variables
                    return Expression.Block(
                        Expression.Call(Expression.Constant(context.Variables()), tryUpdateMethod, Expression.Constant(varName), lResultObj, dictionaryAccess),
                        exp);
                case SyntaxKind.BitwiseNotExpression:
                    return Expression.Not(exp);
                case SyntaxKind.LogicalNotExpression:
                    return Expression.Not(exp);
                case SyntaxKind.UnaryMinusExpression:
                    return Expression.Negate(exp);
                default:
                    throw new CompilationException("Unable to resolve unary expression" + prefixSyntax.Operand);
            }

        }
    }
}
