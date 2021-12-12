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
    public class ElementAccessExpressionTransformerTests
    {
        [Test]
        public void TestElementAccessDictionary()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(Dictionary<int, int>));
            Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestElementAccessArray()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(int[]));
            Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestElementAccessInvalid()
        {
            var tree = CSharpSyntaxTree.ParseText("d[1]", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ElementAccessExpressionSyntax elementAccessSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ElementAccessExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("d", typeof(string));
            try
            {
                Expression cExp = ElementAccessExpressionTransformer.INSTANCE.ToExpression(context, elementAccessSyntax);
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
