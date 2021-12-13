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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpEval.Transformer
{
    public class IdentifierExpressionTransformer : IExpressionTransformer<Expression, IdentifierNameSyntax>
    {
        public static IdentifierExpressionTransformer INSTANCE = new IdentifierExpressionTransformer();

        public Expression ToExpression(Context context, IdentifierNameSyntax identifierSyntax)
        {
            //Each variable is not created natively as a ParameterExpression in Linq.
            //Instead, a single dictionary containing all variables are held.
            //Access to variable is through Element access on the dictionary.
            string identifier = identifierSyntax.Identifier.ValueText;
            var param = context.VariableDeclarations().FirstOrDefault(v => v.Name == identifier);

            if (param == null)
            {
                throw new CompilationException("Unknown variable " + identifier);
            }
            Expression argExpression = Expression.Constant(identifier);
            PropertyInfo pInfo = typeof(ConcurrentDictionary<string, object>).GetProperty("Item", typeof(object));
            Expression expression = Expression.Convert(Expression.Property(Expression.Constant(context.Variables(),
                typeof(ConcurrentDictionary<string, object>)), pInfo, argExpression), param.Type);
            return expression;
        }
    }
}
