﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation
{
    [System.Diagnostics.Contracts.Pure]
    internal sealed class CellParsingInfo : BaseReadOnlyObject
    {
        private readonly ArgumentPrepareAttribute.ArgumentPrepareMode argumentPrepareMode;
        private readonly bool trimInputCharacters;

        public CellParsingInfo(ParameterInfoReference parameter, HtmlCell targetCell)
            : this(targetCell, parameter.ParameterType, parameter.PrepareMode, parameter.TrimInputCharacters)
        {

        }

        public CellParsingInfo(
            HtmlCell targetCell,
            TypeReference expectedType,
            ArgumentPrepareAttribute.ArgumentPrepareMode argumentPrepareMode = ArgumentPrepareAttribute.ArgumentPrepareMode.CleanupHtmlContent,
            bool trimInputCharacters = false)
        {
            Validate.ArgumentIsNotNull(targetCell, "targetCell");
            Validate.ArgumentIsNotNull(expectedType, "expectedType");
            Validate.ArgumentEnumerationValueIsDefined(argumentPrepareMode, "argumentPrepareMode");

            this.argumentPrepareMode = argumentPrepareMode;
            this.trimInputCharacters = trimInputCharacters;
            TargetCell = targetCell;
            ExpectedType = expectedType;
        }

        [NotNull]
        public HtmlCell TargetCell
        {
            get;
            private set;
        }

        [NotNull]
        public TypeReference ExpectedType
        {
            get;
            private set;
        }

        [NotNull]
        public string PrepareString()
        {
            var result = OrganizeHtmlContent();

            if (trimInputCharacters)
            {
                return result.Trim();
            }

            return result;
        }

        private string OrganizeHtmlContent()
        {
            switch (argumentPrepareMode)
            {
                case ArgumentPrepareAttribute.ArgumentPrepareMode.RawHtml:
                    return TargetCell.RawContent;
                case ArgumentPrepareAttribute.ArgumentPrepareMode.CleanupHtmlContent:
                    return TargetCell.CleanedContent;
                default:
                    throw new InvalidOperationException(string.Format("Argument prepare mode {0} is not supported", argumentPrepareMode));
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return trimInputCharacters;
            yield return argumentPrepareMode;
            yield return TargetCell;
            yield return ExpectedType;
        }
    }
}