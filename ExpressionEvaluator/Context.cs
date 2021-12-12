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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionEvaluator
{
    /// <summary>
    /// Expression evaluation state through which variable declaration and
    /// type export are configured. <see cref="Context.DeclareVariable(string, Type)"/>
    /// and <see cref="Context.ExportType(Type)"/>
    /// </summary>
    public class Context
    {
        private List<ParameterExpression> variables = new List<ParameterExpression>();
        private List<Type> exportedTypes = new List<Type>();
        private ConcurrentDictionary<string, object> variableHolder = new ConcurrentDictionary<string, object>();

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

        public ConcurrentDictionary<string, object> Variables()
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
