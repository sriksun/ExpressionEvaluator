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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class PostUnaryExpressionTransformer : IExpressionTransformer<Expression, PostfixUnaryExpressionSyntax>
    {
        public static PostUnaryExpressionTransformer INSTANCE = new PostUnaryExpressionTransformer();

        private readonly MethodInfo tryUpdateMethod;

        public PostUnaryExpressionTransformer()
        {
            tryUpdateMethod = typeof(ConcurrentDictionary<string, object>).GetMethod("TryUpdate");
        }

        public Expression ToExpression(Context context, PostfixUnaryExpressionSyntax postfixSyntax)
        {
            Expression exp = ExpressionFactory.ToExpression(context, postfixSyntax.Operand);

            switch (postfixSyntax.Kind())
            {
                case SyntaxKind.PostDecrementExpression:
                case SyntaxKind.PostIncrementExpression:
                    string varName = null;
                    IndexExpression dictionaryAccess = null;
                    if (exp.NodeType == ExpressionType.Convert)
                    {
                        dictionaryAccess = (IndexExpression)((UnaryExpression)exp).Operand;
                        varName = ((ConstantExpression)(dictionaryAccess.Arguments[0])).Value.ToString();
                    }
                    if (varName == null)
                    {
                        throw new CompilationException("Can't perform postfix inc/dec assignment" + postfixSyntax.Operand);
                    }
                    int delta = postfixSyntax.Kind() == SyntaxKind.PostDecrementExpression ? -1 : 1;
                    Expression lResult = Expression.Add(exp, Expression.Constant(delta));
                    Expression lResultObj = Expression.Convert(lResult, typeof(object));
                    ParameterExpression tmpVar = Expression.Variable(dictionaryAccess.Type, "_tmp" + varName);

                    //Assignment to variable is achieved through update to concurrent dictionary holding all the variables
                    return Expression.Block(
                        exp.Type,
                        new List<ParameterExpression>() { tmpVar },
                        new List<Expression>() {
                            Expression.Assign(tmpVar, dictionaryAccess),
                            Expression.Call(Expression.Constant(context.Variables()), tryUpdateMethod, Expression.Constant(varName), lResultObj, dictionaryAccess),
                            Expression.Convert(tmpVar, exp.Type)}
                        ); 
                default:
                    throw new CompilationException("Unable to resolve unary expression" + postfixSyntax.Operand);
            }

        }
    }
}
