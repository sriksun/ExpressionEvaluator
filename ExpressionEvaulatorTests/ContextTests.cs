using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionEvaluator;
using NUnit.Framework;

namespace ExpressionEvaulatorTests
{
    public class ContextTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDeclareVariable()
        {
            Context context = new Context();
            Assert.AreEqual(context.VariableDeclarations().Count(), 0);
            context.DeclareVariable("a", typeof(int));
            Assert.AreEqual(context.VariableDeclarations().Count(), 1);
            Assert.AreEqual(context.VariableDeclarations().ElementAt(0).Name, "a");
            Assert.AreEqual(context.VariableDeclarations().ElementAt(0).Type, typeof(int));

            context.DeclareVariable("b", typeof(string));
            Assert.AreEqual(context.VariableDeclarations().Count(), 2);
            Assert.AreEqual(context.VariableDeclarations().ElementAt(1).Name, "b");
            Assert.AreEqual(context.VariableDeclarations().ElementAt(1).Type, typeof(string));
        }

        [Test]
        public void TestDeclareVariables()
        {
            Context context = new Context();
            Assert.AreEqual(context.VariableDeclarations().Count(), 0);
            context.DeclareVariable("x", typeof(int));
            Assert.AreEqual(context.VariableDeclarations().Count(), 1);

            Dictionary<string, Type> dictionary = new Dictionary<string, Type>()
            {
                { "a", typeof(int) },
                { "b", typeof(string) }
            };
            context.DeclareVariables(dictionary);
            Assert.AreEqual(context.VariableDeclarations().Count(), 2);
            foreach (ParameterExpression pExp in context.VariableDeclarations())
            {
                Assert.AreEqual(dictionary[pExp.Name], pExp.Type);
            }
        }

        [Test]
        public void TestExportType()
        {
            Context context = new Context();
            Assert.AreEqual(context.ExportedTypes().Count(), 0);
            context.ExportType(typeof(int));
            Assert.AreEqual(context.ExportedTypes().Count(), 1);
            Assert.AreEqual(context.ExportedTypes().ElementAt(0), typeof(int));

            context.ExportType(typeof(DateTime));
            Assert.AreEqual(context.ExportedTypes().Count(), 2);
            Assert.AreEqual(context.ExportedTypes().ElementAt(1), typeof(DateTime));
        }

        [Test]
        public void TestExportTypes()
        {
            Context context = new Context();
            Assert.AreEqual(context.ExportedTypes().Count(), 0);
            context.ExportType(typeof(int));
            Assert.AreEqual(context.ExportedTypes().Count(), 1);

            Type[] aTypes = new Type[] { typeof(string), typeof(double) };
            context.ExportTypes(aTypes);
            Assert.AreEqual(context.ExportedTypes().Count(), 2);

            Assert.False(context.ExportedTypes().Contains(typeof(int)));
            Assert.True(context.ExportedTypes().Contains(typeof(string)));
            Assert.True(context.ExportedTypes().Contains(typeof(double)));

            aTypes = new Type[] { typeof(byte), typeof(ulong) };
            context.ExportTypes(aTypes.ToList());
            Assert.AreEqual(context.ExportedTypes().Count(), 2);

            Assert.True(context.ExportedTypes().Contains(typeof(byte)));
            Assert.True(context.ExportedTypes().Contains(typeof(ulong)));
        }

        [Test]
        public void TestSetVariable()
        {
            Context context = new Context();
            Assert.AreEqual(context.Variables().Count(), 0);
            context.SetVariable("a", 50);
            Assert.AreEqual(context.Variables().Count(), 1);
            Assert.AreEqual(context.Variables()["a"], 50);

            context.SetVariable("b", "abc");
            Assert.AreEqual(context.Variables().Count(), 2);
            Assert.AreEqual(context.Variables()["b"], "abc");
        }

        [Test]
        public void TestSetVariables()
        {
            Context context = new Context();
            Assert.AreEqual(context.Variables().Count(), 0);
            context.SetVariable("x", 500);
            Assert.AreEqual(context.Variables().Count(), 1);

            Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "a", 50 },
                { "b", "abc" }
            };
            context.SetVariables(dictionary);
            Assert.AreEqual(context.Variables().Count(), 2);
            foreach (KeyValuePair<string, object> var in context.Variables())
            {
                Assert.AreEqual(dictionary[var.Key], var.Value);
            }
        }

    }
}
