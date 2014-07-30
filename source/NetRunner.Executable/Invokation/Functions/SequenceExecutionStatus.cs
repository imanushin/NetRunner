using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Invokation.Remoting;
using NetRunner.Executable.RawData;
using NetRunner.ExternalLibrary.Properties;
using NetRunner.TestExecutionProxy;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class SequenceExecutionStatus
    {
        public SequenceExecutionStatus()
        {
            AllIsOk = true;
            Changes = new List<AbstractTableChange>();
        }

        public List<AbstractTableChange> Changes
        {
            get;
            private set;
        }

        public bool AllIsOk
        {
            get;
            set;
        }

        public bool WereExceptions
        {
            get;
            set;
        }

        public void MergeWith(ExecutionResult other)
        {
            AllIsOk &= !other.HasError;
            WereExceptions |= other.HasError;
        }

        [StringFormatMethod("generalInfoFormat")] 
        public void MergeWith(ExecutionResult other, HtmlCell problematicCell, string generalInfoFormat, params object[] args)
        {
            AllIsOk &= !other.HasError;
            WereExceptions |= other.HasError;

            if (other.HasError)
            {
                Changes.Add(new CssClassCellChange(problematicCell, HtmlParser.ErrorCssClass));
                Changes.Add(new AddCellExpandableInfo(problematicCell, string.Format(generalInfoFormat, args), other.ExceptionToString));
            }
        }

        public void MergeWith(TableChangeCollection other)
        {
            AllIsOk &= other.AllWasOk;
            WereExceptions |= other.WereExceptions;
            Changes.AddRange(other.Changes);
        }

        public bool CompareAndMerge(GeneralIsolatedReference first, GeneralIsolatedReference second, HtmlCell targetCell)
        {
            var areEquals = first.EqualsSafe(second);

            MergeWith(areEquals, targetCell, "Unable to compare {0} and {1}", first.GetType().GetData().Name, second.GetType().GetData().Name);

            return !areEquals.HasError && areEquals.Result.IsTrue;
        }
    }
}
