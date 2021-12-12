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

using System.Linq.Expressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    /// <summary>
    /// Transforms a Roslyn CSharp Expression Syntax <see cref="ExpressionSyntax"/> to
    /// corresponding Expression <see cref="Expression"/>
    /// </summary>
    /// <typeparam name="A">of type Expression <see cref="Expression"/></typeparam>
    /// <typeparam name="B">of type Expression Syntax <see cref="ExpressionSyntax"/></typeparam>
    public interface IExpressionTransformer<A, B> where B : ExpressionSyntax where A : Expression
    {
        A ToExpression(Context context, B expressionSyntax) ;
    }
}
