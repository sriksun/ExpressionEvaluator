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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator
{
    public class ExpressionCompiler
    {
        private readonly Context context;

        public ExpressionCompiler(Context context) 
        {
            this.context = context;
        }

        internal SyntaxTree ValidateExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression))
                throw new CompilationException("Expression cannot be empty or null");

            var tree = CSharpSyntaxTree.ParseText(expression, CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            var diagnostics = tree.GetDiagnostics().ToList();

            if (diagnostics.Count > 0)
                throw new CompilationException(diagnostics[0].ToString());

            return tree;
        }

        public CompiledExpression Compile(string expression)
        {
            ExpressionStatementSyntax expr = ValidateExpression(expression)
                .GetRoot()
                .DescendantNodes()
                .OfType<ExpressionStatementSyntax>()
                .First();

            Expression exp = ExpressionFactory.ToExpression(context, expr.ChildNodes().OfType<ExpressionSyntax>().First());

            return new CompiledExpression(context, Expression.Block(
                context.VariableDeclarations(),
                exp
            ));
        }
    }
}
