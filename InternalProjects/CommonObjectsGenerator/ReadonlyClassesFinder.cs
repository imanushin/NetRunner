using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonObjectsGenerator
{
    public static class ReadonlyClassesFinder
    {
        private static readonly Dictionary<Type, bool> readonlyTypes = new Dictionary<Type, bool>();

        public static IEnumerable<Type> FindTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(IsTypeReadOnly);
        }

        public static bool IsTypeReadOnly(Type type)
        {
            if (type.IsGenericType)
                return false;

            if (readonlyTypes.ContainsKey(type))
                return readonlyTypes[type];
            
            Type baseType = type.BaseType;
            while(baseType != null)
            {
                if (baseType.FullName == "LeakBlocker.Libraries.Common.BaseReadOnlyObject")
                    break;
                baseType = baseType.BaseType;
            }

            bool result = baseType != null;

            readonlyTypes[type] = result;

            return result;
        }

        public static string ConvertToTestNamespace(string objectNamespace)
        {
            int indexOfFirstDot = objectNamespace.IndexOf('.');
            int indexOfSecondDot = objectNamespace.IndexOf('.', indexOfFirstDot + 1);
            int indexOfThirdDot = objectNamespace.IndexOf('.', indexOfSecondDot + 1);

            if (indexOfThirdDot == -1)
                return objectNamespace + ".Tests";

            string baseProjectName = objectNamespace.Substring(0, indexOfThirdDot);
            string afterThirdDot = objectNamespace.Substring(indexOfThirdDot + 1);

            const string testNamespaceTemplate = "{0}.Tests.{1}";

            return string.Format(testNamespaceTemplate, baseProjectName, afterThirdDot);
        }

    }
}
