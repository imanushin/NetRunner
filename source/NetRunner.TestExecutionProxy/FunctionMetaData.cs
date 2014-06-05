﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class FunctionMetaData : MarshalByRefObject
    {
        private readonly object targetObject;

        public FunctionMetaData(MethodInfo method, object targetObject)
        {
            this.targetObject = targetObject;
            Method = method;

            AvailableFunctionNames = GetFunctionNamesAvailable(method);
        }

        private static ReadOnlyCollection<string> GetFunctionNamesAvailable(MethodInfo method)
        {
            var result = new List<string>();

            var allMarkers = method.GetCustomAttributes<AdditionalFunctionNameAttribute>().ToArray();

            result.AddRange(allMarkers.OfType<AdditionalFunctionNameAttribute>().Select(a => a.Name));

            var parentType = method.DeclaringType;
            var assembly = parentType.Assembly;

            var excludeDefaultName = method.GetCustomAttributes<ExcludeDefaultFunctionNameAttribute>()
                .Concat(parentType.GetCustomAttributes<ExcludeDefaultFunctionNameAttribute>())
                .Concat(assembly.GetCustomAttributes<ExcludeDefaultFunctionNameAttribute>())
                .FirstOrDefault();

            if (excludeDefaultName == null || !excludeDefaultName.ExcludeDefaultFunctionName)
            {
                result.Add(method.Name);
            }

            return result.AsReadOnly();
        }

        internal MethodInfo Method
        {
            [Pure]
            get;
            private set;
        }

        public ReadOnlyCollection<string> AvailableFunctionNames
        {
            [Pure]
            get;
            private set;
        }

        public string SystemName
        {
            [Pure]
            get
            {
                return Method.Name;
            }
        }

        public TypeReference ReturnType
        {
            [Pure]
            get
            {
                return new TypeReference(Method.ReturnType);
            }
        }

        public int ParametersCount
        {
            [Pure]
            get
            {
                return Method.GetParameters().Length;
            }
        }

        [Pure]
        public ParameterInfoReference[] GetParameters()
        {
            return Method.GetParameters().Select(p => new ParameterInfoReference(p)).ToArray();
        }

        public ExecutionResult Invoke(IsolatedReference<object>[] parameters)
        {
            try
            {
                var result = Method.Invoke(targetObject, parameters.Select(p => TrimStrings(p.Value)).ToArray());

                return new ExecutionResult(new IsolatedReference<object>(result));
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        private static object TrimStrings(object value)
        {
            var strValue = value as string;

            if (GlobalSettings.TrimAllInputLines && strValue != null)
            {
                return strValue.Trim();
            }

            return value;
        }
    }
}
