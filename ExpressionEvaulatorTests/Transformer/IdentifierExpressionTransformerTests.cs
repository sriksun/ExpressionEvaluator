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
using ExpressionEvaluator;
using ExpressionEvaluator.Transformer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace ExpressionEvaulatorTests.Transformer
{
    public class IdentifierExpressionTransformerTests
    {
        [Test]
        public void TestValidVariable()
        {
            var tree = CSharpSyntaxTree.ParseText("a", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            IdentifierNameSyntax identifierSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(DateTime));
            Expression cExp = IdentifierExpressionTransformer.INSTANCE.ToExpression(context, identifierSyntax);
            Assert.AreEqual(cExp.Type, typeof(DateTime));
        }

        [Test]
        public void TestInvalidVariable()
        {
            var tree = CSharpSyntaxTree.ParseText("b", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            IdentifierNameSyntax identifierSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(DateTime));
            try
            {
                Expression cExp = IdentifierExpressionTransformer.INSTANCE.ToExpression(context, identifierSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }
    }
}
