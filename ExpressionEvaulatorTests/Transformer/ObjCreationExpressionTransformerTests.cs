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
    public class ObjCreationExpressionTransformerTests
    {
        [Test]
        public void TestUnexportTypeObjCreation()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime()", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            try
            {
                NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestObjCreationWrongConstructor()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime(false)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            context.ExportType(typeof(DateTime));
            try
            {
                NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(CompilationException));
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void TestObjCreation()
        {
            var tree = CSharpSyntaxTree.ParseText("new DateTime(50000000000)", CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            ObjectCreationExpressionSyntax objCreationSyntax = tree
                .GetRoot()
                .DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .First();

            Context context = new Context();
            context.ExportType(typeof(DateTime));
            NewExpression cExp = ObjectCreationExpressionTransformer.INSTANCE.ToExpression(context, objCreationSyntax);
            Assert.AreEqual(cExp.NodeType, ExpressionType.New);
            Assert.AreEqual(cExp.Type, typeof(DateTime));
        }
    }
}
