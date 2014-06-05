using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    internal sealed class TestFunctionReference : BaseReadOnlyObject
    {
        public TestFunctionReference(FunctionMetaData method, GeneralIsolatedReference targetObject)
        {
            Validate.ArgumentIsNotNull(method, "method");
            Validate.ArgumentIsNotNull(targetObject, "targetObject");

            Method = method;
            TargetObject = targetObject;

            ArgumentTypes = method.GetParameters().ToReadOnlyList();
            ResultType = method.ReturnType;
            AvailableFunctionNames = method.AvailableFunctionNames.Select(CleanFunctionName).ToReadOnlyList();
        }

        public FunctionMetaData Method
        {
            get;
            private set;
        }

        public ReadOnlyList<string> AvailableFunctionNames
        {
            get;
            private set;
        }

        public GeneralIsolatedReference TargetObject
        {
            get;
            private set;
        }

        public ReadOnlyList<ParameterInfoReference> ArgumentTypes
        {
            get;
            private set;
        }

        public TypeReference ResultType
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get
            {
                return TargetObject.GetType().Name + '.' + Method.SystemName;
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Method.SystemName;
            yield return ArgumentTypes;
            yield return TargetObject;
            yield return ResultType;
            yield return AvailableFunctionNames;
        }

        protected override string GetString()
        {
            return string.Format("Method: {0}; target object type: {1}; Parameters: {2}; Result type: {3}; AvailableFunctionNames: {4}", Method.SystemName, TargetObject.GetType().Name, ArgumentTypes, ResultType, AvailableFunctionNames);
        }

        public ExecutionResult Invoke(IEnumerable<IsolatedReference<object>> parameters)
        {
            return Method.Invoke(parameters.ToArray());
        }

        [NotNull, Pure]
        public static string CleanFunctionName([NotNull]string functionName)
        {
            Validate.ArgumentStringIsMeanful(functionName, "functionName");

            return functionName
                .Replace(" ", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\t", string.Empty);
        }
    }
}
