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
using ExpressionEvaluator;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class CompilationExceptionTests
    {
        [Test]
        public void TestCompilationException()
        {
            Exception inner = new Exception("Inner");
            string message = "Test Exception";

            CompilationException e = new CompilationException();
            Assert.AreEqual(e.Message, "Exception of type '" + e.GetType().FullName + "' was thrown.");
            Assert.AreEqual(e.InnerException, null);

            e = new CompilationException(message);
            Assert.AreEqual(e.Message, message);
            Assert.AreEqual(e.InnerException, null);

            e = new CompilationException(message, inner);
            Assert.AreEqual(e.Message, message);
            Assert.AreEqual(e.InnerException, inner);
        }
    }
}
