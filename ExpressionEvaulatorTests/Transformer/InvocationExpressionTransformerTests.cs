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
using ExpEval;
using ExpEval.Transformer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace ExpressionEvaulatorTests.Transformer
{
    public class InvocationExpressionTransformerTests
    {
        [Test]
        public void TestInvocation()
        {
            var tree = CSharpSyntaxTree.ParseText("a.Substring(1)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("a", typeof(string));
            MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
            Assert.AreEqual(cExp.Type, typeof(string));
        }

        [Test]
        public void TestMemberAccessInvalidMethod1()
        {
            var tree = CSharpSyntaxTree.ParseText("a.Substring(1,2,3)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("a", typeof(string));
            try
            {
                MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestMemberAccessInvalidMethod2()
        {
            var tree = CSharpSyntaxTree.ParseText("a.Unknown()", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.DeclareVariable("a", typeof(string));
            try
            {
                MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestMemberAccessExportedMethod()
        {
            var tree = CSharpSyntaxTree.ParseText("Int32.Parse(\"100\")", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.ExportType(typeof(Int32));
            MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
            Assert.AreEqual(cExp.Type, typeof(Int32));
        }

        [Test]
        public void TestMemberAccessExportedMethodError1()
        {
            var tree = CSharpSyntaxTree.ParseText("Int32.Parse(1,2,3)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.ExportType(typeof(Int32));
            try
            {
                MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestMemberAccessExportedMethodError2()
        {
            var tree = CSharpSyntaxTree.ParseText("Int32.ParseA()", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            InvocationExpressionSyntax invocationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .First();
            Context context = new Context();
            context.ExportType(typeof(Int32));
            try
            {
                MethodCallExpression cExp = InvocationExpressionTransformer.INSTANCE.ToExpression(context, invocationSyntax);
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
