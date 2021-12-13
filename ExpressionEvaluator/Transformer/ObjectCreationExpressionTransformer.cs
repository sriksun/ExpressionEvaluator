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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpEval.Transformer
{
    public class ObjectCreationExpressionTransformer : IExpressionTransformer<NewExpression, ObjectCreationExpressionSyntax>
    {
        public static ObjectCreationExpressionTransformer INSTANCE = new ObjectCreationExpressionTransformer();

        public NewExpression ToExpression(Context context, ObjectCreationExpressionSyntax objectCreationSyntax)
        {
            var suppliedArgs = objectCreationSyntax.ArgumentList.Arguments;
            int argsLength = suppliedArgs.Count();
            Expression[] arguments = new Expression[argsLength];

            for (int i = 0; i < argsLength; i++)
                arguments[i] = ExpressionFactory.ToExpression(context, suppliedArgs[i].Expression);

            string typeName = objectCreationSyntax.Type.ToString();
            Type type = context.ExportedTypes().FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);

            if (type == null)
                throw new CompilationException(string.Format("The type or namespace name '{0}' could not be found", typeName));

            Type[] types = new Type[argsLength];
            for (int i = 0; i < argsLength; i++)
            {
                types[i] = arguments[i].Type;
            }
            ConstructorInfo constructor = type.GetTypeInfo().GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, null, types, null);
            if (constructor != null)
            {
                try
                {
                    return Expression.New(constructor, arguments);
                }
                catch (System.Exception)
                {
                    throw new CompilationException("Unable to create a new expression with this constructor");
                }
            }
            throw new CompilationException(string.Format("Constructor with specified arguments could not be found on type '{0}'", typeName));
        }
    }
}
