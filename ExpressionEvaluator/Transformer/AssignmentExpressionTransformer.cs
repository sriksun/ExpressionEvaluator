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

namespace ExpEval.Transformer
{
    public class AssignmentExpressionTransformer : IExpressionTransformer<Expression, AssignmentExpressionSyntax>
    {
        public static AssignmentExpressionTransformer INSTANCE = new AssignmentExpressionTransformer();

        private readonly MethodInfo tryUpdateMethod;

        public AssignmentExpressionTransformer()
        {
            tryUpdateMethod = typeof(ConcurrentDictionary<string, object>).GetMethod("TryUpdate");
        }
        

        public Expression ToExpression(Context context, AssignmentExpressionSyntax assignmentSyntax)
        {
            Expression lExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Left);
            Expression rExp = ExpressionFactory.ToExpression(context, assignmentSyntax.Right);

            string varName = null;
            IndexExpression dictionaryAccess = null;
            if (lExp.NodeType == ExpressionType.Convert)
            {
                dictionaryAccess = (IndexExpression)((UnaryExpression)lExp).Operand;
                varName = ((ConstantExpression)(dictionaryAccess.Arguments[0])).Value.ToString();
            }
            if (varName == null)
            {
                throw new CompilationException("Can't perform assignment ");
            }
            Expression lResult;
            switch (assignmentSyntax.Kind())
            {
                case SyntaxKind.AddAssignmentExpression:
                    lResult = Expression.Add(lExp, rExp);
                    break;
                case SyntaxKind.SubtractAssignmentExpression:
                    lResult = Expression.Subtract(lExp, rExp);
                    break;
                case SyntaxKind.MultiplyAssignmentExpression:
                    lResult = Expression.Multiply(lExp, rExp);
                    break;
                case SyntaxKind.DivideAssignmentExpression:
                    lResult = Expression.Divide(lExp, rExp);
                    break;
                case SyntaxKind.ModuloAssignmentExpression:
                    lResult = Expression.Modulo(lExp, rExp);
                    break;
                case SyntaxKind.AndAssignmentExpression:
                    lResult = Expression.And(lExp, rExp);
                    break;
                case SyntaxKind.OrAssignmentExpression:
                    lResult = Expression.Or(lExp, rExp);
                    break;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    lResult = Expression.ExclusiveOr(lExp, rExp);
                    break;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    lResult = Expression.LeftShift(lExp, rExp);
                    break;
                case SyntaxKind.RightShiftAssignmentExpression:
                    lResult = Expression.RightShift(lExp, rExp);
                    break;
                default:
                    lResult = rExp;
                    break;
            }
            //Assignment to variable is achieved through update to concurrent dictionary holding all the variables
            Expression lResultObj = Expression.Convert(lResult, typeof(object));
            return Expression.Block(
                Expression.Call(Expression.Constant(context.Variables()), tryUpdateMethod, Expression.Constant(varName), lResultObj, dictionaryAccess),
                lExp);
        }
    }
}