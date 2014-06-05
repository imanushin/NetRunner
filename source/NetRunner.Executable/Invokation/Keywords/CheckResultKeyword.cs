using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Keywords
{
    internal sealed class CheckResultKeyword : AbstractKeyword
    {
        private readonly ReadOnlyList<HtmlCell> patchedCells;
        private readonly HtmlCell lastCell;
        private readonly ReadOnlyList<AbstractTableChange> parsingErrors;

        private CheckResultKeyword(ReadOnlyList<HtmlCell> patchedCells, HtmlCell lastCell, ReadOnlyList<AbstractTableChange> parsingErrors)
        {
            Validate.ArgumentIsNotNull(patchedCells, "patchedCells");
            Validate.ArgumentIsNotNull(lastCell, "lastCell");
            Validate.ArgumentIsNotNull(parsingErrors, "parsingErrors");

            this.patchedCells = patchedCells;
            this.lastCell = lastCell;
            this.parsingErrors = parsingErrors;
        }

        public override ReadOnlyList<HtmlCell> PatchedCells
        {
            get
            {
                return patchedCells;
            }
        }

        public override ReadOnlyList<AbstractTableChange> ParsingErrors
        {
            get
            {
                return parsingErrors;
            }
        }

        public override InvokationResult InvokeFunction(Func<InvokationResult> func, TestFunctionReference targetFunction)
        {
            if (parsingErrors.Any())
            {
                return new InvokationResult(null, new TableChangeCollection(false, true, parsingErrors));
            }

            var result = base.InvokeFunction(func, targetFunction);

            if (result.Changes.WereExceptions)
                return result;

            var resultObject = result.Result;

            var resultType = targetFunction.ResultType;

            string errorHeader = string.Format("Unable to convert result to '{0}'", resultType.Name);

            var expectedObject = ParametersConverter.ConvertParameter(lastCell, resultType, errorHeader);

            if (expectedObject.Changes.WereExceptions)
            {
                return expectedObject;
            }

            bool checkSucceded = Equals(expectedObject.Result, resultObject);

            if (checkSucceded)
            {
                return new InvokationResult(ReflectionLoader.TrueResult);
            }

            var showActualValueCellChange = new ShowActualValueCellChange(lastCell, resultObject);

            return new InvokationResult(ReflectionLoader.TrueResult, new TableChangeCollection(false, false, showActualValueCellChange));
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

            var cellsInTheMiddle = inputCells.Skip(1).Take(inputCells.Count - 2).ToReadOnlyList();

            if (lastCell.IsBold)
            {
                const string header = "Last cell should not be bold";
                const string info = "Last cell should have value (e.g. last cell should not be bold). Example: | check | '''int default result ''' | 0 |";

                var cellInfo = new AddCellExpandableInfo(lastCell, header, info);

                var changes = inputCells.Select(c => new CssClassCellChange(c, HtmlParser.ErrorCssClass)).Cast<AbstractTableChange>().ToList();

                changes.Add(cellInfo);

                return new CheckResultKeyword(cellsInTheMiddle, lastCell, changes.ToReadOnlyList());
            }

            Validate.CollectionHasElements(cellsInTheMiddle, "There are not any cells between check keyword and last cell");

            return new CheckResultKeyword(cellsInTheMiddle, lastCell, ReadOnlyList<AbstractTableChange>.Empty);
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return patchedCells;
            yield return lastCell;
            yield return parsingErrors;
        }
    }
}
