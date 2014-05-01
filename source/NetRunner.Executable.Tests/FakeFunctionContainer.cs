using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Tests
{
    internal sealed class FakeFunctionContainer : BaseTestContainer
    {
        public FakeFunctionContainer(int index)
        {
            Index = index;
        }

        public int Index
        {
            get;
            private set;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FakeFunctionContainer;

            if (other == null)
                return false;

            return other.Index == Index;
        }

        public override int GetHashCode()
        {
            return Index;
        }
    }
}
