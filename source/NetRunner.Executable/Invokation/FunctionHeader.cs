﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation
{
    internal sealed class FunctionHeader : BaseReadOnlyObject
    {
        public FunctionHeader(string functionName, IEnumerable<HtmlCell> arguments, HtmlRowReference rowReference, ReadOnlyList<HtmlCell> functionCells, AbstractKeyword keyword)
        {
            Validate.ArgumentStringIsMeanful(functionName, "functionName");
            Validate.ArgumentIsNotNull(arguments, "arguments");
            Validate.ArgumentIsNotNull(keyword, "keyword");
            Validate.ArgumentIsNotNull(rowReference, "rowReference");
            Validate.CollectionArgumentHasElements(functionCells, "functionCells");

            FunctionName = TestFunctionReference.CleanFunctionName(functionName);
            Arguments = arguments.ToReadOnlyList();
            RowReference = rowReference;
            Keyword = keyword;
            FunctionCells = functionCells;
        }

        public ReadOnlyList<HtmlCell> FunctionCells
        {
            get;
            private set;
        }

        public HtmlRowReference RowReference
        {
            get;
            private set;
        }

        public AbstractKeyword Keyword
        {
            get;
            private set;
        }

        public string FunctionName
        {
            get;
            private set;
        }

        public ReadOnlyList<HtmlCell> Arguments
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return RowReference;
            yield return FunctionName;
            yield return Arguments;
            yield return Keyword;
            yield return FunctionCells;
        }

        protected override string GetString()
        {
            return string.Format("FunctionName: {0}, Arguments: {1}, Keyword: {2}", FunctionName, Arguments, Keyword);
        }
    }
}
