using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class ObjectCreationExpressionTransformer : Transformer<NewExpression, ObjectCreationExpressionSyntax>
    {
        public static ObjectCreationExpressionTransformer INSTANCE = new ObjectCreationExpressionTransformer();

        public NewExpression ToExpression(Context context, ObjectCreationExpressionSyntax objectCreationSyntax)
        {
            var suppliedArgs = objectCreationSyntax.ArgumentList.Arguments;
            int argsLength = suppliedArgs.Count();
            Expression[] arguments = new Expression[argsLength];

            for (int i = 0; i < argsLength; i++)
                arguments[i] = ExpressionFactory.ToExpression(context, suppliedArgs[i].Expression);

            string typeName = objectCreationSyntax.Type.ToString();
            Type type = context.ExportedTypes().FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);

            if (type == null)
                throw new CompilationException(string.Format("The type or namespace name '{0}' could not be found", typeName));

            var constructors = type.GetTypeInfo().DeclaredConstructors;
            foreach (ConstructorInfo constructor in constructors)
            {
                try
                {
                    return Expression.New(constructor, arguments);
                }
                catch (System.Exception) { }
            }
            throw new CompilationException(string.Format("Constructor with specified arguments could not be found on type '{0}'", typeName));
        }
    }
}
