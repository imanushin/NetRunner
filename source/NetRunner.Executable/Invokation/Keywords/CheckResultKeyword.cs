using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal sealed class CheckResultKeyword : AbstractKeyword
    {
        private readonly ReadOnlyList<HtmlCell> patchedCells;
        private readonly HtmlCell lastCell;

        private CheckResultKeyword(ReadOnlyList<HtmlCell> patchedCells, HtmlCell lastCell)
        {
            Validate.CollectionArgumentHasElements(patchedCells, "patchedCells");
            Validate.ArgumentIsNotNull(lastCell, "lastCell");

            this.patchedCells = patchedCells;
            this.lastCell = lastCell;
        }

        public override ReadOnlyList<HtmlCell> PatchedCells
        {
            get
            {
                return patchedCells;
            }
        }

        public override InvokationResult InvokeFunction(Func<InvokationResult> func, TestFunctionReference targetFunction)
        {
            var result = base.InvokeFunction(func, targetFunction);

            if (result.ExecutedWithErrors)
                return result;

            var resultObject = result.Result;

            var expectedObject = ParametersConverter.ConvertParameter(lastCell.CleanedContent, targetFunction.ResultType);

            bool checkSucceded = Equals(expectedObject, resultObject);

            if (checkSucceded)
            {
                return new InvokationResult(true);
            }
            
            var showActualValueCellChange = new ShowActualValueCellChange(lastCell, resultObject);

            return new InvokationResult(false, false, showActualValueCellChange);
        }

        /// <summary>
        /// Example: 
        /// | check | '''string empty result''' |  |
        /// or
        /// | check | '''int default result ''' | 0 |
        /// </summary>
        /// <param name="inputCells"></param>
        /// <returns></returns>
        [CanBeNull]
        public static CheckResultKeyword TryParse(IReadOnlyCollection<HtmlCell> inputCells)
        {
            if (inputCells.Count < 3)
                return null;

            var firstCell = inputCells.First().CleanedContent.Trim();

            if (!string.Equals(firstCell, "check", StringComparison.OrdinalIgnoreCase))
                return null;

            var lastCell = inputCells.Last();

            Validate.Condition(!lastCell.IsBold, "Last cell should have value (e.g. last cell should not be bold). Example: | check | '''int default result ''' | 0 |");

            var cellsInTheMiddle = inputCells.Skip(1).Take(inputCells.Count - 2).ToReadOnlyList();

            return new CheckResultKeyword(cellsInTheMiddle, lastCell);
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return patchedCells;
            yield return lastCell;
        }
    }
}
