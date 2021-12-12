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

namespace ExpressionEvaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            CompiledExpression cExp = new ExpressionCompiler(context).Compile("a+=3");
            ExpressionEvaluator evaluator = new ExpressionEvaluator(cExp);
            evaluator.SetVariable("a", 5);
            Func<int> func = evaluator.Evaluate<int>();
            Console.WriteLine(func());
            Console.WriteLine(context.Variables()["a"]);
        }
    }
}
