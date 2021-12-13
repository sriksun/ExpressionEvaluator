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
using System.Linq.Expressions;

namespace ExpEval
{
    /// <summary>
    /// Holds internal state of a compiled expression. Only a compiled
    /// expression can be evaluated. ExpressionCompiler <see cref="ExpressionCompiler.Compile(string)"/>
    /// is used to build a CompiledExpression object used for evalutation.
    /// </summary>
    public class CompiledExpression<T>
    {
        private readonly Context context;
        private readonly Func<T> funcHandle;

        public CompiledExpression(Context context, Func<T> funcHandle)
        {
            this.context = context;
            this.funcHandle = funcHandle;
        }

        public Func<T> FunctionDelegate()
        {
            return funcHandle;
        }

        public Context Context()
        {
            return context;
        }
    }
}
