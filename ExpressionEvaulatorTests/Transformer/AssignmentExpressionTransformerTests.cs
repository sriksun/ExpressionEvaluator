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
    public class AssignmentExpressionTransformerTests
    {
        [Test]
        public void TestAdd()
        {
            var tree = CSharpSyntaxTree.ParseText("a+=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestAssign()
        {
            var tree = CSharpSyntaxTree.ParseText("a=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestAddError()
        {
            var tree = CSharpSyntaxTree.ParseText("5+=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            } catch(CompilationException)
            {
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestSubtract()
        {
            var tree = CSharpSyntaxTree.ParseText("a-=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestMultiply()
        {
            var tree = CSharpSyntaxTree.ParseText("a*=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestDivide()
        {
            var tree = CSharpSyntaxTree.ParseText("a/=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestModulo()
        {
            var tree = CSharpSyntaxTree.ParseText("a%=3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestLeftShift()
        {
            var tree = CSharpSyntaxTree.ParseText("a <<= 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestRightShift()
        {
            var tree = CSharpSyntaxTree.ParseText("a >>= 1", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseAnd()
        {
            var tree = CSharpSyntaxTree.ParseText("a &= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseOr()
        {
            var tree = CSharpSyntaxTree.ParseText("a |= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }

        [Test]
        public void TestBitwiseXOr()
        {
            var tree = CSharpSyntaxTree.ParseText("a ^= 3", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            AssignmentExpressionSyntax assignmentSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .First();

            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            Expression cExp = AssignmentExpressionTransformer.INSTANCE.ToExpression(context, assignmentSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.Block);
            Assert.AreEqual(cExp.Type, typeof(int));
        }
    }
}
