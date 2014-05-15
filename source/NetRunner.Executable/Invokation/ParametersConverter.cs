using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal static class ParametersConverter
    {
        private static readonly Dictionary<Type, BaseParser> parsers = new Dictionary<Type, BaseParser>();
        private static readonly MethodInfo tryParseMethod = typeof(BaseParser).GetMethod("TryParse");

        public static InvokationResult ConvertParameter(HtmlCell inputData, Type expectedType, string conversionErrorHeader)
        {
            BaseParser parser;

            var errorCellMark = new CssClassCellChange(inputData, HtmlParser.ErrorCssClass);

            if (!parsers.TryGetValue(expectedType, out parser))
            {
                foreach (var baseParser in ReflectionLoader.Parsers)
                {
                    var parseResult = ParseData(inputData, expectedType, conversionErrorHeader, baseParser);

                    if (parseResult != null)
                        return parseResult;
                }

                var parserNotFound = new AddCellExpandableInfo(inputData, conversionErrorHeader, string.Format("Unable to find parser for type {0}. Parsers available: {1}", expectedType, ReflectionLoader.Parsers));

                return new InvokationResult(null, new TableChangeCollection(false, true, parserNotFound, errorCellMark));
            }

            var foundParserResult = ParseData(inputData, expectedType, conversionErrorHeader, parser);

            if (foundParserResult != null)
                return foundParserResult;

            var noParserCellChange = new AddCellExpandableInfo(inputData, conversionErrorHeader, string.Format("Internal error: Object {0} parser type {1} earlier however it could not do this now.", parser, expectedType));

            return new InvokationResult(null, new TableChangeCollection(false, true, noParserCellChange, errorCellMark));
        }

        [CanBeNull]
        private static InvokationResult ParseData(HtmlCell inputData, Type expectedType, string conversionErrorHeader, BaseParser baseParser)
        {
            try
            {
                object result;

                if (TryParseData(inputData.CleanedContent, expectedType, baseParser, out result))
                {
                    parsers[expectedType] = baseParser;

                    return new InvokationResult(result);
                }
            }
            catch (TargetInvocationException ex)
            {
                var cellChange = new AddCellExpandableException(inputData, ex.InnerException, conversionErrorHeader);

                return new InvokationResult(null, new TableChangeCollection(false, true, cellChange));
            }
            return null;
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
