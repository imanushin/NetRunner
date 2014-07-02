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
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    internal static class ParametersConverter
    {
        private static readonly Dictionary<TypeReference, IsolatedParser> parsers = new Dictionary<TypeReference, IsolatedParser>();

        public static void ResetParsers()
        {
            parsers.Clear();
        }

        public static InvokationResult ConvertParameter(CellParsingInfo cellInfo, string conversionErrorHeader)
        {
            IsolatedParser parser;

            var inputData = cellInfo.TargetCell;
            var expectedType = cellInfo.ExpectedType;

            var errorCellMark = new CssClassCellChange(inputData, HtmlParser.ErrorCssClass);

            if (!parsers.TryGetValue(expectedType, out parser))
            {
                foreach (var baseParser in ReflectionLoader.Parsers)
                {
                    var parseResult = ParseData(cellInfo, conversionErrorHeader, baseParser);

                    if (parseResult != null)
                        return parseResult;
                }

                var parserNotFound = new AddCellExpandableInfo(inputData, conversionErrorHeader, string.Format("Unable to find parser for type {0}. Parsers available: {1}", expectedType, ReflectionLoader.Parsers));

                return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, parserNotFound, errorCellMark));
            }

            var foundParserResult = ParseData(cellInfo, conversionErrorHeader, parser);

            if (foundParserResult != null)
                return foundParserResult;

            var noParserCellChange = new AddCellExpandableInfo(inputData, conversionErrorHeader, string.Format("Internal error: Object {0} parsed type {1} earlier however it could not do this now.", parser, expectedType));

            return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, noParserCellChange, errorCellMark));
        }

        [CanBeNull]
        private static InvokationResult ParseData(CellParsingInfo cellInfo, string conversionErrorHeader, IsolatedParser baseParser)
        {
            var inputData = cellInfo.TargetCell;
            var expectedType = cellInfo.ExpectedType;

            try
            {
                ExecutionResult result;

                var parseSucceeded = baseParser.TryParse(cellInfo.PrepareString(), expectedType, out result);

                if (result.HasError)
                {
                    var cellChange = new AddCellExpandableException(inputData, result, conversionErrorHeader);

                    return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, cellChange));
                }

                if (parseSucceeded)
                {
                    parsers[expectedType] = baseParser;

                    return InvokationResult.CreateSuccessResult(result);
                }
            }
            catch (TargetInvocationException ex)
            {
                var cellChange = new AddCellExpandableException(inputData, ex.InnerException, conversionErrorHeader);

                return InvokationResult.CreateErrorResult(new TableChangeCollection(false, true, cellChange));
            }
            return null;
        }

    }
}
