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

using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class LiteralExpressionTransformer : Transformer<ConstantExpression, LiteralExpressionSyntax>
    {
        public static LiteralExpressionTransformer INSTANCE = new LiteralExpressionTransformer();

        public ConstantExpression ToExpression(Context context, LiteralExpressionSyntax literalSyntax)
        {
            string literal = literalSyntax.GetText().ToString();
            switch (literalSyntax.Kind())
            {
                case SyntaxKind.NumericLiteralExpression:
                    int intVal;
                    if (!int.TryParse(literal, out intVal))
                    {
                        long longVal;
                        if (!long.TryParse(literal, out longVal))
                        {
                            double doubleVal;
                            if (!double.TryParse(literal, out doubleVal))
                            {
                                throw new CompilationException("Unable to parse Numeric literal " + literal);
                            }
                            return Expression.Constant(doubleVal, typeof(double));

                        }
                        return Expression.Constant(longVal, typeof(long));
                    }
                    return Expression.Constant(intVal, typeof(int));
                case SyntaxKind.StringLiteralExpression:
                    return Expression.Constant(literal.TrimStart('"').TrimEnd('"'), typeof(string));
                case SyntaxKind.CharacterLiteralExpression:
                    return Expression.Constant(literal.TrimStart('\'').TrimEnd('\'')[0], typeof(char));
                case SyntaxKind.TrueLiteralExpression:
                    return Expression.Constant(true, typeof(bool));
                case SyntaxKind.FalseLiteralExpression:
                    return Expression.Constant(false, typeof(bool));
                case SyntaxKind.NullLiteralExpression:
                    return Expression.Constant(null);
                default:
                    throw new CompilationException("Unknown literal " + literal); ;
            }
        }
    }
}
