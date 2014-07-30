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
    public sealed class FunctionMetaData : GeneralReferenceObject
    {
        private readonly Lazy<GeneralIsolatedReference> targetObject;

        private FunctionMetaData([NotNull] MethodInfo method, [NotNull] Lazy<GeneralIsolatedReference> targetObject, TypeReference objectType)
        {
            Method = method;
            this.targetObject = targetObject;
            MethodReference = MethodReference.GetMethod(method);
        }

        internal FunctionMetaData([NotNull] MethodInfo method, [NotNull] GeneralLazyIsolatedReference targetObject)
            : this(method, new Lazy<GeneralIsolatedReference>(() => targetObject.Instance), targetObject.Type)
        {
        }

        internal FunctionMetaData([NotNull] MethodInfo method, [NotNull] GeneralIsolatedReference targetObject)
            : this(method, new Lazy<GeneralIsolatedReference>(() => targetObject), targetObject.GetType())
        {
        }

        internal MethodInfo Method
        {
            [Pure]
            get;
            private set;
        }

        public MethodReference MethodReference
        {
            get;
            private set;
        }

        [NotNull]
        public ExecutionResult Invoke(ParameterValue[] parameters)
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

                var result = Method.Invoke(targetObject.Value.Value, parametersArray);

                var outParameters = ExtractOutParameters(originalParameters, parametersArray);

                var actualResult = new ExecutionResult(new GeneralIsolatedReference(result, TypeReference.GetType(Method.ReturnType)), outParameters);

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

        private static object[] PrepareParameters(ParameterValue[] parameters, ParameterInfo[] originalParameters)
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

        private static List<ParameterValue> ExtractOutParameters(ParameterInfo[] originalParameters, object[] parametersArray)
        {
            var outParameters = new List<ParameterValue>();

            for (int i = 0; i < originalParameters.Length; i++)
            {
                var parameter = originalParameters[i];

                if (!parameter.IsOut)
                {
                    continue;
                }

                outParameters.Add(new ParameterValue(parameter.Name, new GeneralIsolatedReference(parametersArray[i], originalParameters[i].ParameterType)));
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
            try
            {
                var objectForExecute = targetObject.Value.Value as TTargetType;

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
            catch (Exception ex)
            {
                return ExecutionResult.FromException(ex);
            }
        }

        private ExecutionResult Execute(Func<FunctionContainer, MethodInfo, Exception> function)
        {
            return ExecuteHandler(function, Method);
        }

        public ParameterInfoReference GetParameter(string name)
        {
            var result = Method.GetParameters()
                .Where(p => string.Equals(name, p.Name, StringComparison.OrdinalIgnoreCase))
                .Select(p => ParameterInfoReference.GetParameter(p, MethodReference))
                .FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException(string.Format("Unable to find parameter {0} of method {1}", name, Method));

            return result;
        }


    }
}
