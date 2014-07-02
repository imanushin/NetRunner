using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void MergeWith(TableChangeCollection other)
        {
            AllIsOk &= other.AllWasOk;
            WereExceptions |= other.WereExceptions;
            Changes.AddRange(other.Changes);
        }
    }
}
