using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// The default parser. For the input type it tries to find public static T Parse(string). Type is not supported by this parser if the method is abcent. 
    /// Otherwise parser will be this method to parse any input line.<br/>
    /// Therefore many of the default types are supported, such as <see cref="int"/>, <see cref="double"/>, <see cref="System.DateTime"/>, etc.
    /// </summary>
    internal sealed class DefaultParser : BaseParser
    {
        private readonly object syncRoot = new object();

        private readonly Dictionary<Type, MethodInfo> parseMethods = new Dictionary<Type, MethodInfo>();
        private readonly HashSet<Type> notSupportedTypes = new HashSet<Type>();

        public DefaultParser()
            : base(Priorities.EmbeddedParsersPriority)
        {
        }

        public override bool TryParse<TResult>(string value, out TResult parsedResult)
        {
            var targetType = typeof(TResult);

            MethodInfo parseMethod;

            lock (syncRoot)
            {
                if (notSupportedTypes.Contains(targetType))
                {
                    parsedResult = default(TResult);

                    return false;
                }

                if (!parseMethods.TryGetValue(targetType, out parseMethod))
                {
                    parseMethod = targetType.GetMethods().FirstOrDefault(m => IsParseMethod(m, targetType));

                    if (parseMethod == null)
                    {
                        notSupportedTypes.Add(targetType);

                        parsedResult = default(TResult);

                        return false;
                    }

                    parseMethods[targetType] = parseMethod;
                }
            }

            parsedResult = (TResult)parseMethod.Invoke(null, new object[]
            {
                value
            });

            return true;
        }
        private static bool IsParseMethod(MethodInfo method, Type expectedType)
        {
            if (!string.Equals(method.Name, "Parse", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!method.IsStatic)
                return false;

            if (!expectedType.IsAssignableFrom(method.ReturnType))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 1)
                return false;

            var firstParameter = parameters.First();

            if (typeof(string) != firstParameter.ParameterType)
                return false;

            return true;
        }
    }
}
