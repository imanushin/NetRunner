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
        public ExecutionResult Invoke(GeneralIsolatedReference[] parameters)
        {
            var executeBefore = ExecuteBeforeFunctionCallMethod();

            if (executeBefore.HasError)
            {
                return executeBefore;
            }

            try
            {
                var result = Method.Invoke(targetObject, parameters.Select(p => p.Value).ToArray());

                var actualResult = new ExecutionResult(new GeneralIsolatedReference(result));

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
