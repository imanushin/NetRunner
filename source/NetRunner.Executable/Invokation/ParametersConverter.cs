﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Invokation
{
    internal static class ParametersConverter
    {
        private static readonly Dictionary<Type, BaseParser> parsers = new Dictionary<Type, BaseParser>();
        private static readonly MethodInfo tryParseMethod = typeof(BaseParser).GetMethod("TryParse");

        public static object ConvertParameter(string inputData, Type expectedType)
        {
            BaseParser parser;
            object result;

            if (!parsers.TryGetValue(expectedType, out parser))
            {
                foreach (var baseParser in ReflectionLoader.Instance.Parsers)
                {
                    try
                    {
                        if (TryParseData(inputData, expectedType, baseParser, out result))
                        {
                            parsers[expectedType] = baseParser;

                            return result;
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw new ConversionException(expectedType, inputData, ex.InnerException);
                    }
                }

                throw new InvalidOperationException(string.Format("Unable to find parser for type {0}. Parsers available: {1}", expectedType, ReflectionLoader.Instance.Parsers));
            }

            bool resultWasParsed;

            try
            {
                resultWasParsed = TryParseData(inputData, expectedType, parser, out result);
            }
            catch (TargetInvocationException ex)
            {
                throw new ConversionException(expectedType, inputData, ex.InnerException);
            }

            Validate.Condition(resultWasParsed, "Internal error: Object {0} parser type {1} earlier however it could not do this now.", parser, expectedType);

            return result;
        }

        private static bool TryParseData(string inputData, Type expectedType, BaseParser parser, out object result)
        {
            MethodInfo generic = tryParseMethod.MakeGenericMethod(expectedType);

            var args = new object[]
            {
                inputData, null
            };

            var parseResult = (bool)generic.Invoke(parser, args);

            result = args[1];

            return parseResult;
        }
    }
}
