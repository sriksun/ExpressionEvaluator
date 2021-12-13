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
    public class PrefixUnaryExpressionTransformerTests
    {
        [Test]
        public void TestPrefixDecrement()
        {
            var tree = CSharpSyntaxTree.ParseText("--a", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixIncrement()
        {
            var tree = CSharpSyntaxTree.ParseText("++a", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixNegate()
        {
            var tree = CSharpSyntaxTree.ParseText("-5", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = (UnaryExpression)PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Negate);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, 5);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixNot()
        {
            var tree = CSharpSyntaxTree.ParseText("~7", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = (UnaryExpression)PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Not);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, 7);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestPrefixLogicalNot()
        {
            var tree = CSharpSyntaxTree.ParseText("!true", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            PrefixUnaryExpressionSyntax prefixSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<PrefixUnaryExpressionSyntax>()
                .First();

            Context context = new Context();
            UnaryExpression cExp = (UnaryExpression)PrefixUnaryExpressionTransformer.INSTANCE.ToExpression(context, prefixSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Not);
            Assert.AreEqual(((ConstantExpression)cExp.Operand).Value, true);
            Assert.AreEqual(cExp.Type, typeof(bool));
        }
    }
}
