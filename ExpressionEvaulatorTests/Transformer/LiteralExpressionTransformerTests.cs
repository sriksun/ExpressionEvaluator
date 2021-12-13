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
    public class LiteralExpressionTransformerTests
    {
        [Test]
        public void TestNumericLiteralInt()
        {
            var tree = CSharpSyntaxTree.ParseText("5", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context(); 
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 5);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestNumericLiteralDouble()
        {
            var tree = CSharpSyntaxTree.ParseText("5.1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 5.1);
            Assert.AreEqual(cExp.Type, typeof(double));
        }

        [Test]
        public void TestNumericLiteralException()
        {
            var tree = CSharpSyntaxTree.ParseText("5dhfg", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            } catch (Exception ignore)
            {
                Assert.AreEqual(ignore.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestLiteralString()
        {
            var tree = CSharpSyntaxTree.ParseText("\"hello\"", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, "hello");
            Assert.AreEqual(cExp.Type, typeof(string));
        }

        [Test]
        public void TestLiteralChar()
        {
            var tree = CSharpSyntaxTree.ParseText("'h'", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, 'h');
            Assert.AreEqual(cExp.Type, typeof(char));
        }

        [Test]
        public void TestLiteralTueBool()
        {
            var tree = CSharpSyntaxTree.ParseText("true", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, true);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLiteralFalseBool()
        {
            var tree = CSharpSyntaxTree.ParseText("false", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, false);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }

        [Test]
        public void TestLiteralNull()
        {
            var tree = CSharpSyntaxTree.ParseText("null", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            LiteralExpressionSyntax literalSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .First();

            Context context = new Context();
            ConstantExpression cExp = LiteralExpressionTransformer.INSTANCE.ToExpression(context, literalSyntax);
            Assert.AreEqual(cExp.Value, null);
            Assert.AreEqual(cExp.Type, typeof(object));
        }

    }
}
