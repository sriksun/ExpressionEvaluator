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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class BinaryExpressionTransformer : IExpressionTransformer<BinaryExpression, BinaryExpressionSyntax>
    {
        public static BinaryExpressionTransformer INSTANCE = new BinaryExpressionTransformer();

        private Dictionary<Type, int> priority = new Dictionary<Type, int>()
        {
            { typeof(byte), 1 },
            { typeof(short), 2 },
            { typeof(int), 3 },
            { typeof(long), 4 },
            { typeof(float), 5 },
            { typeof(double), 6 }
        };

        public BinaryExpression ToExpression(Context context, BinaryExpressionSyntax binarySyntax)
        {
            Expression lExp = ExpressionFactory.ToExpression(context, binarySyntax.Left);
            Expression rExp = ExpressionFactory.ToExpression(context, binarySyntax.Right);
            int lTypePrio = priority.ContainsKey(lExp.Type) ? priority[lExp.Type] : 0;
            int rTypePrio = priority.ContainsKey(rExp.Type) ? priority[rExp.Type] : 0;

            if (lTypePrio > 0 && rTypePrio > 0 & lTypePrio != rTypePrio)
            {
                if (lTypePrio < rTypePrio)
                {
                    lExp = Expression.Convert(lExp, rExp.Type);
                }
                else
                {
                    rExp = Expression.Convert(rExp, lExp.Type);
                }
            }

            switch (binarySyntax.Kind())
            {
                case SyntaxKind.AddExpression:
                    return Expression.Add(lExp, rExp);
                case SyntaxKind.SubtractExpression:
                    return Expression.Subtract(lExp, rExp);
                case SyntaxKind.MultiplyExpression:
                    return Expression.Multiply(lExp, rExp);
                case SyntaxKind.DivideExpression:
                    return Expression.Divide(lExp, rExp);
                case SyntaxKind.ModuloExpression:
                    return Expression.Modulo(lExp, rExp);
                case SyntaxKind.EqualsExpression:
                    return Expression.Equal(lExp, rExp);
                case SyntaxKind.NotEqualsExpression:
                    return Expression.NotEqual(lExp, rExp);
                case SyntaxKind.LessThanExpression:
                    return Expression.LessThan(lExp, rExp);
                case SyntaxKind.LessThanOrEqualExpression:
                    return Expression.LessThanOrEqual(lExp, rExp);
                case SyntaxKind.GreaterThanExpression:
                    return Expression.GreaterThan(lExp, rExp);
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return Expression.GreaterThanOrEqual(lExp, rExp);
                case SyntaxKind.LogicalAndExpression:
                    if ((lTypePrio + rTypePrio) > 0)
                    {
                        return Expression.NotEqual(Expression.And(lExp, rExp), Expression.Constant(0));
                    }
                    return Expression.And(lExp, rExp);
                case SyntaxKind.LogicalOrExpression:
                    if ((lTypePrio + rTypePrio) > 0)
                    {
                        return Expression.NotEqual(Expression.Or(lExp, rExp), Expression.Constant(0));
                    }
                    return Expression.Or(lExp, rExp);
                case SyntaxKind.CoalesceExpression:
                    return Expression.Coalesce(lExp, rExp);
                case SyntaxKind.BitwiseAndExpression:
                    return Expression.And(lExp, rExp);
                case SyntaxKind.BitwiseOrExpression:
                    return Expression.Or(lExp, rExp);
                case SyntaxKind.ExclusiveOrExpression:
                    return Expression.ExclusiveOr(lExp, rExp);
                case SyntaxKind.LeftShiftExpression:
                    return Expression.LeftShift(lExp, rExp);
                case SyntaxKind.RightShiftExpression:
                    return Expression.RightShift(lExp, rExp);
                default:
                    throw new CompilationException("Unknown binary expression" + binarySyntax.GetText());
            }
        }
    }
}
