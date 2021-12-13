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

namespace ExpEval
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context();
            context.DeclareVariable("a", typeof(int));
            CompiledExpression<int> cExp = new ExpressionCompiler(context).Compile<int>("a+=3");
            ExpressionEvaluator<int> evaluator = new ExpressionEvaluator<int>(cExp);
            
            int val = evaluator.Evaluate(new Dictionary<string, object>() { { "a", 5} });
            Console.WriteLine(val);
            Console.WriteLine(context.Variables()["a"]);
        }
    }
}
