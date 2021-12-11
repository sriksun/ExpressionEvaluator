using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    public class Context
    {
        private List<ParameterExpression> variables = new List<ParameterExpression>();
        private List<Type> exportedTypes = new List<Type>();
        private Dictionary<string, object> variableHolder = new Dictionary<string, object>();

        public Context DeclareVariable(string name, Type type)
        {
            ParameterExpression exp = Expression.Variable(type, name);
            variables.Add(exp);
            return this;
        }

        public Context DeclareVariables(Dictionary<string, Type> variableDeclarations)
        {
            variables.Clear();
            foreach (KeyValuePair<string, Type> declaration in variableDeclarations)
            {
                DeclareVariable(declaration.Key, declaration.Value);
            }
            return this;
        }

        public Context ExportType(Type type)
        {
            exportedTypes.Add(type);
            return this;
        }

        public Context ExportTypes(Type[] types)
        {
            exportedTypes.Clear();
            exportedTypes.AddRange(types.ToList());
            return this;
        }

        public Context ExportTypes(List<Type> types)
        {
            exportedTypes.Clear();
            exportedTypes.AddRange(types);
            return this;
        }

        public List<Type> ExportedTypes()
        {
            return exportedTypes;
        }

        public List<ParameterExpression> VariableDeclarations()
        {
            return variables;
        }

        public Dictionary<string, object> Variables()
        {
            return variableHolder;
        }

        public void SetVariable(string name, object val)
        {
            variableHolder[name]= val;
        }

        public void SetVariables(Dictionary<string, object> variables)
        {
            variableHolder.Clear();
            foreach (KeyValuePair<string, object> variable in variables)
            {
                variableHolder[variable.Key] = variable.Value;
            }
        }
    }
}
