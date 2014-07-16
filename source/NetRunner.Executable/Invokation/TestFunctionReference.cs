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
        private static readonly char[] incorrectFunctionCharacters = " \n\r\t!@#$%^&*()_+=-/*`~\\|/,.№:;\"'?<>[]{}".ToCharArray();

        internal TestFunctionReference(FunctionMetaData method, TypeReference owner)
        {
            Validate.ArgumentIsNotNull(method, "method");

            Method = method;
            Owner = owner;

            Arguments = method.GetParameters().ToReadOnlyList();
            ResultType = method.ReturnType;
            AvailableFunctionNames = method.AvailableFunctionNames.Select(CleanFunctionName).ToReadOnlyList();
        }

        [NotNull]
        public FunctionMetaData Method
        {
            get;
            private set;
        }

        [NotNull]
        public TypeReference Owner
        {
            get;
            private set;
        }

        [NotNull]
        public ReadOnlyList<string> AvailableFunctionNames
        {
            get;
            private set;
        }

        [NotNull]
        public ReadOnlyList<ParameterInfoReference> Arguments
        {
            get;
            private set;
        }

        [NotNull]
        public TypeReference ResultType
        {
            get;
            private set;
        }

        [NotNull]
        public string DisplayName
        {
            get
            {
                return Method.ObjectType.Name + '.' + Method.SystemName;
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Method.SystemName;
            yield return Arguments;
            yield return ResultType;
            yield return Owner;
            yield return AvailableFunctionNames;
        }

        protected override string GetString()
        {
            return string.Format("Method: {0};  Parameters: {1}; Result type: {2}; AvailableFunctionNames: {3}", Method.SystemName, Arguments, ResultType, AvailableFunctionNames);
        }

        [NotNull]
        public ExecutionResult Invoke(IEnumerable<ParameterData> parameters)
        {
            return Method.Invoke(parameters.ToArray());
        }

        [NotNull, Pure]
        public static string CleanFunctionName([NotNull]string functionName)
        {
            Validate.ArgumentStringIsMeanful(functionName, "functionName");

            var subNames = functionName.Split(incorrectFunctionCharacters, StringSplitOptions.RemoveEmptyEntries);

            return string.Concat(subNames);
            ;
        }
    }
}
