using System;
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

        public CellParsingInfo(ParameterInfoReference parameter, HtmlCell targetCell)
            : this(targetCell, parameter.ParameterType, parameter.PrepareMode)
        {

        }

        public CellParsingInfo(
            HtmlCell targetCell,
            TypeReference expectedType,
            ArgumentPrepareAttribute.ArgumentPrepareMode argumentPrepareMode = ArgumentPrepareAttribute.ArgumentPrepareMode.CleanupHtmlContent)
        {
            Validate.ArgumentIsNotNull(targetCell, "targetCell");
            Validate.ArgumentIsNotNull(expectedType, "expectedType");
            Validate.ArgumentEnumerationValueIsDefined(argumentPrepareMode, "argumentPrepareMode");

            this.argumentPrepareMode = argumentPrepareMode;
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
            yield return argumentPrepareMode;
            yield return TargetCell;
            yield return ExpectedType;
        }
    }
}
