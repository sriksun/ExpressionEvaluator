using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ExpressionEvaluator.Transformer
{
    public class MemberAccessExpressionTransformer : Transformer<MemberExpression, MemberAccessExpressionSyntax>
    {
        public static MemberAccessExpressionTransformer INSTANCE = new MemberAccessExpressionTransformer();

        public MemberExpression ToExpression(Context context, MemberAccessExpressionSyntax memberAccessSyntax)
        {
            if (memberAccessSyntax.Expression.GetType() == typeof(IdentifierNameSyntax))
            {
                IdentifierNameSyntax identifierNode = (IdentifierNameSyntax)memberAccessSyntax.Expression;
                string identifier = identifierNode.Identifier.ValueText;

                Type type = context.ExportedTypes().FirstOrDefault(v => v.Name == identifier);
                if (type != null)
                {
                    MemberInfo[] mInfos = type.GetMember(memberAccessSyntax.Name.Identifier.ValueText);
                    if (mInfos.Length > 0)
                    {
                        return Expression.MakeMemberAccess(null, mInfos[0]);
                    }
                    else
                    {
                        throw new CompilationException("Unsupported property or field: " + memberAccessSyntax.Name.Identifier.ValueText);
                    }
                }
            }
            try
            {
                return Expression.PropertyOrField(ExpressionFactory.ToExpression(context, memberAccessSyntax.Expression), memberAccessSyntax.Name.Identifier.ValueText);
            }
            catch (Exception e)
            {
                throw new CompilationException("Unsupported property or field: " + memberAccessSyntax.Name.Identifier.ValueText);
            }
        }
    }
}
