using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class TableChangeCollection : BaseReadOnlyObject
    {
        private static readonly TableChangeCollection allIsOkNoChanges = new TableChangeCollection(true, false, ReadOnlyList<AbstractTableChange>.Empty);

        public static TableChangeCollection AllIsOkNoChanges
        {
            get
            {
                return allIsOkNoChanges;
            }
        }

        public TableChangeCollection(bool allWasOk, bool wereExceptions, params AbstractTableChange[] changes)
            :this(allWasOk,wereExceptions, changes.ToReadOnlyList())
        {
        }

        public TableChangeCollection(bool allWasOk, bool wereExceptions, IReadOnlyCollection<AbstractTableChange> changes)
        {
            Validate.ArgumentIsNotNull(changes, "changes");

            AllWasOk = allWasOk;
            WereExceptions = wereExceptions;
            Changes = changes.ToReadOnlyList();
        }

        public bool AllWasOk
        {
            get;
            private set;
        }

        public bool WereExceptions
        {
            get;
            private set;
        }

        public ReadOnlyList<AbstractTableChange> Changes
        {
            get;
            private set;
        }


        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return AllWasOk;
            yield return WereExceptions;
            yield return Changes;
        }
    }
}
