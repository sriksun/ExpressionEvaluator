﻿using System;
namespace ExpressionEvaluator
{
    public class CompilationException : Exception
    {
		CompilationException() : base()
        {
        }

		public CompilationException(string? message) : base(message)
        {
        }

        public CompilationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

	}
}