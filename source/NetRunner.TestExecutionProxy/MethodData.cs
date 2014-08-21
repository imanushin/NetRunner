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
    [Serializable]
    public sealed class MethodData : IHelpIdentity
    {
        private const string methodIdentityFormat = "M:{0}.{1}({2})";

        public MethodData(MethodInfo method, MethodReference methodReference)
        {
            Parameters = method.GetParameters().Select(p => ParameterInfoReference.GetParameter(p, methodReference)).ToList().AsReadOnly();
            SystemName = method.Name;
            ReturnType = TypeReference.GetType(method.ReturnType);
            Owner = TypeReference.GetType(method.DeclaringType);


            AvailableFunctionNames = GetFunctionNamesAvailable(method);

            HelpIdentity = string.Format(
                methodIdentityFormat,
                ReflectionHelpers.GetTypeNameWithoutGenerics(method.DeclaringType),
                SystemName,
                string.Join(",", method.GetParameters().Select(a => ReplaseRefSymbol(ReflectionHelpers.GetTypeNameWithoutGenerics(a.ParameterType)))));

        }

        public ReadOnlyCollection<ParameterInfoReference> Parameters
        {
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
            get;
            private set;
        }

        public TypeReference ReturnType
        {
            get;
            private set;
        }

        public TypeReference Owner
        {
            get;
            private set;
        }

        public string HelpIdentity
        {
            get;
            private set;
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

        private static string ReplaseRefSymbol(string typeName)
        {
            if (typeName.EndsWith("&"))
            {
                return typeName.Substring(0, typeName.Length - 1) + "@";
            }

            return typeName;
        }

    }
}
