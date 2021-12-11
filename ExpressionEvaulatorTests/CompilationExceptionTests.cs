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
