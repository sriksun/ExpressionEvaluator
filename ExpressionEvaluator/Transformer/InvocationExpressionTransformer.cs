using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class InvocationExpressionTransformer : Transformer<MethodCallExpression, InvocationExpressionSyntax>
    {
        public static InvocationExpressionTransformer INSTANCE = new InvocationExpressionTransformer();

        public MethodCallExpression ToExpression(Context context, InvocationExpressionSyntax invocationSyntax)
        {
            var suppliedArgs = invocationSyntax.ArgumentList.Arguments;
            int argsLength = suppliedArgs.Count();
            Expression[] arguments = new Expression[argsLength];
            Type[] types = new Type[arguments.Length];

            for (int i = 0; i < argsLength; i++)
            {
                arguments[i] = ExpressionFactory.ToExpression(context, suppliedArgs[i].Expression);
                types[i] = arguments[i].Type;
            }

            ExpressionSyntax invoker = invocationSyntax.Expression;
            if (invoker.Kind() == SyntaxKind.SimpleMemberAccessExpression)
            {
                string methodName = ((MemberAccessExpressionSyntax)invoker).Name.Identifier.ValueText;
                string obj = ((MemberAccessExpressionSyntax)invoker).Expression.ToString();
                Type type = context.ExportedTypes().FirstOrDefault(v => v.Name == obj);
                if (type == null)
                {
                    Expression instance = ExpressionFactory.ToExpression(context, ((MemberAccessExpressionSyntax)invoker).Expression);
                    MethodInfo methodInfo = instance.Type.GetMethod(methodName, types);
                    if (methodInfo != null)
                    {
                        try
                        {
                            return Expression.Call(instance, methodInfo, arguments);
                        } catch(Exception)
                        {
                            throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                        }
                    }
                    else
                    {
                        throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                    }
                }
                else
                {
                    MethodInfo methodInfo = type.GetMethod(methodName, types);
                    if (methodInfo != null)
                    {
                        try
                        {
                            return Expression.Call(methodInfo, arguments);
                        } catch (Exception)
                        {
                            throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                        }
                    }
                    else
                    {
                        throw new CompilationException("Method doesn't exist " + methodName + " with specific argTypes");
                    }
                }
            }
            throw new CompilationException("Unsupported Expression " + invocationSyntax.GetText());
        }
    }
}
