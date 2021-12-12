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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class InvocationExpressionTransformer : Transformer<MethodCallExpression, InvocationExpressionSyntax>
    {
        public static InvocationExpressionTransformer INSTANCE = new InvocationExpressionTransformer();

        public MethodCallExpression ToExpression(Context context, InvocationExpressionSyntax invocationSyntax)
        {
            var suppliedArgs = invocationSyntax.ArgumentList.Arguments;
            int argsLength = suppliedArgs.Count();
            Expression[] arguments = new Expression[argsLength];
            Type[] types = new Type[arguments.Length];

            for (int i = 0; i < argsLength; i++)
            {
                arguments[i] = ExpressionFactory.ToExpression(context, suppliedArgs[i].Expression);
                types[i] = arguments[i].Type;
            }

            ExpressionSyntax invoker = invocationSyntax.Expression;
            if (invoker.Kind() == SyntaxKind.SimpleMemberAccessExpression)
            {
                string methodName = ((MemberAccessExpressionSyntax)invoker).Name.Identifier.ValueText;
                string obj = ((MemberAccessExpressionSyntax)invoker).Expression.ToString();
                Type type = context.ExportedTypes().FirstOrDefault(v => v.Name == obj);
                if (type == null)
                {
                    Expression instance = ExpressionFactory.ToExpression(context, ((MemberAccessExpressionSyntax)invoker).Expression);
                    MethodInfo methodInfo = instance.Type.GetMethod(methodName, types);
                    if (methodInfo != null)
                    {
                        try
                        {
                            return Expression.Call(instance, methodInfo, arguments);
                        } catch(Exception)
                        {
                            throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                        }
                    }
                    else
                    {
                        throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                    }
                }
                else
                {
                    MethodInfo methodInfo = type.GetMethod(methodName, types);
                    if (methodInfo != null)
                    {
                        try
                        {
                            return Expression.Call(methodInfo, arguments);
                        } catch (Exception)
                        {
                            throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                        }
                    }
                    else
                    {
                        throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                    }
                }
            }
            throw new CompilationException("Unsupported Expression " + invocationSyntax.GetText());
        }
    }
}
