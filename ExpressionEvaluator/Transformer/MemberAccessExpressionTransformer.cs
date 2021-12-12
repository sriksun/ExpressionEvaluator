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

namespace ExpressionEvaluator.Transformer
{
    public class MemberAccessExpressionTransformer : IExpressionTransformer<MemberExpression, MemberAccessExpressionSyntax>
    {
        public static MemberAccessExpressionTransformer INSTANCE = new MemberAccessExpressionTransformer();

        public MemberExpression ToExpression(Context context, MemberAccessExpressionSyntax memberAccessSyntax)
        {
            if (memberAccessSyntax.Expression.GetType() == typeof(IdentifierNameSyntax))
            {
                IdentifierNameSyntax identifierNode = (IdentifierNameSyntax)memberAccessSyntax.Expression;
                string identifier = identifierNode.Identifier.ValueText;

                Type type = context.ExportedTypes().FirstOrDefault(v => v.Name == identifier);
                if (type != null)
                {
                    MemberInfo[] mInfos = type.GetMember(memberAccessSyntax.Name.Identifier.ValueText);
                    if (mInfos.Length > 0)
                    {
                        return Expression.MakeMemberAccess(null, mInfos[0]);
                    }
                    else
                    {
                        throw new CompilationException("Unsupported property or field: " + memberAccessSyntax.Name.Identifier.ValueText);
                    }
                }
            }
            try
            {
                return Expression.PropertyOrField(ExpressionFactory.ToExpression(context, memberAccessSyntax.Expression), memberAccessSyntax.Name.Identifier.ValueText);
            }
            catch (Exception)
            {
                throw new CompilationException("Unsupported property or field: " + memberAccessSyntax.Name.Identifier.ValueText);
            }
        }
    }
}
