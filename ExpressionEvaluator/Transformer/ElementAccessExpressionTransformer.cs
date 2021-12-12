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
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class ElementAccessExpressionTransformer : IExpressionTransformer<Expression, ElementAccessExpressionSyntax>
    {
        public static ElementAccessExpressionTransformer INSTANCE = new ElementAccessExpressionTransformer();

        public Expression ToExpression(Context context, ElementAccessExpressionSyntax elementAccessSyntax)
        {
            Expression argExpression = ExpressionFactory.ToExpression(context, elementAccessSyntax.ArgumentList.Arguments[0].Expression);
            Expression exp = ExpressionFactory.ToExpression(context, elementAccessSyntax.Expression);
            PropertyInfo propInfo = exp.Type.GetProperty("Item", new Type[] { argExpression.Type });
            if (propInfo != null)
            {
                try
                {
                    IndexExpression expression = Expression.Property(exp, propInfo, argExpression);
                    return expression;
                } catch(ArgumentException)
                {
                    try
                    {
                        IndexExpression expression = Expression.Property(exp, propInfo, Expression.Convert(argExpression, typeof(object)));
                        return expression;
                    }
                    catch (Exception)
                    {
                        throw new CompilationException("Element Access error " + elementAccessSyntax.GetText());
                    }
                }
            } else if (exp.Type.IsArray && argExpression.Type == typeof(int))
            {
                BinaryExpression expression = Expression.ArrayIndex(exp, argExpression);
                return expression;
            }
            throw new CompilationException("Element Access error " + elementAccessSyntax.GetText());
        }
    }
}
