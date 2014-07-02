using System;
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

        internal FunctionMetaData(MethodInfo method, object targetObject)
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
                return TypeReference.GetType(Method.ReturnType);
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

        [NotNull]
        public ExecutionResult Invoke(ParameterData[] parameters)
        {
            var executeBefore = ExecuteBeforeFunctionCallMethod();

            if (executeBefore.HasError)
            {
                return executeBefore;
            }

            try
            {
                var originalParameters = Method.GetParameters();

                var parametersArray = PrepareParameters(parameters, originalParameters);

                var result = Method.Invoke(targetObject, parametersArray);

                var outParameters = ExtractOutParameters(originalParameters, parametersArray);

                var actualResult = new ExecutionResult(new GeneralIsolatedReference(result), outParameters);

                var afterExecutionResult = ExecuteAfterFunctionCallMethod();

                if (afterExecutionResult.HasError)
                {
                    return afterExecutionResult;
                }

                return actualResult;
            }
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        private static object[] PrepareParameters(ParameterData[] parameters, ParameterInfo[] originalParameters)
        {
            var parametersArray = new object[originalParameters.Length];

            for (int i = 0; i < originalParameters.Length; i++)
            {
                var parameter = originalParameters[i];

                if (parameter.IsOut)
                {
                    continue;
                }

                var parameterValue = parameters.FirstOrDefault(p => string.Equals(p.Name, parameter.Name, StringComparison.Ordinal));

                if (parameterValue == null)
                {
                    throw new InvalidOperationException(string.Format(
                        "Internal execution error: parameter {0} is required, however parameter value is undefined. Input values are available for parameters: {1}",
                        parameter.Name,
                        string.Join(", ", parameters.Select(p => p.Name))));
                }

                parametersArray[i] = parameterValue.Value.Value;
            }
            return parametersArray;
        }

        private static List<ParameterData> ExtractOutParameters(ParameterInfo[] originalParameters, object[] parametersArray)
        {
            var outParameters = new List<ParameterData>();

            for (int i = 0; i < originalParameters.Length; i++)
            {
                var parameter = originalParameters[i];

                if (!parameter.IsOut)
                {
                    continue;
                }

                outParameters.Add(new ParameterData(parameter.Name, new GeneralIsolatedReference(parametersArray[i])));
            }
            return outParameters;
        }

        public ExecutionResult ExecuteAfterFunctionCallMethod()
        {
            return Execute((fc, m) => fc.NotifyAfterFunctionCall(m));
        }

        public ExecutionResult ExecuteBeforeFunctionCallMethod()
        {
            return Execute((fc, m) => fc.NotifyBeforeFunctionCall(m));
        }

        [NotNull]
        internal ExecutionResult ExecuteHandler<TArg, TTargetType>(
            Func<TTargetType, TArg, Exception> function,
            TArg argument)
            where TTargetType : FunctionContainer
        {
            var objectForExecute = targetObject as TTargetType;

            if (ReferenceEquals(objectForExecute, null))
            {
                return ExecutionResult.Empty;
            }

            var result = function(objectForExecute, argument);

            if (result != null)
            {
                return ExecutionResult.FromException(result);
            }

            return ExecutionResult.Empty;
        }

        private ExecutionResult Execute(Func<FunctionContainer, MethodInfo, Exception> function)
        {
            return ExecuteHandler(function, Method);
        }
    }
}
