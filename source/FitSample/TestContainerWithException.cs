using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class TestContainerWithConstructorException : BaseTestContainer
    {
        public TestContainerWithConstructorException()
        {
            throw new InvalidOperationException("Surprise!!!");
        }

        public void DoMethodFromProblematicContainer()
        {

        }
    }
}
