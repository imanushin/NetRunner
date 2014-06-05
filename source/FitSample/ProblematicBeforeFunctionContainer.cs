using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class ProblematicBeforeFunctionContainer : BaseTestContainer
    {
        public ProblematicBeforeFunctionContainer()
        {
            BeforeFunctionCall += OnBeforeFunctionCall;
            AfterFunctionCall += OnAfterFunctionCall;
        }

        public BaseTableArgument CreateProblematicArgument()
        {
            return new ProblematicBaseTableArgument();
        }

        public void FailBefore()
        {
        }


        public void FailAfter()
        {
        }

        private void OnAfterFunctionCall(MethodInfo method)
        {
            if (method.Name == "FailAfter")
            {
                throw new InvalidOperationException("OnAfterFunctionCallFail");
            }
        }

        private void OnBeforeFunctionCall(MethodInfo method)
        {
            if (method.Name == "FailBefore")
            {
                throw new InvalidOperationException("OnBeforeFunctionCallFail");
            }
        }

        private sealed class ProblematicBaseTableArgument : BaseTableArgument
        {
            public ProblematicBaseTableArgument()
            {
                BeforeAllFunctionsCall += ProblematicBaseTableArgument_BeforeAllFunctionsCall;
                AfterAllFunctionsCall += ProblematicBaseTableArgument_AfterAllFunctionsCall;
            }

            public void FailBeforeAll(string arg)
            {
            }


            public void FailAfterAll(string arg)
            {
            }

            private void ProblematicBaseTableArgument_AfterAllFunctionsCall(MethodInfo method)
            {
                if (method.Name == "FailAfterAll")
                {
                    throw new InvalidOperationException("AfterAllFunctionsCallFail");
                }
            }

            void ProblematicBaseTableArgument_BeforeAllFunctionsCall(MethodInfo method)
            {
                if (method.Name == "FailBeforeAll")
                {
                    throw new InvalidOperationException("BeforeAllFunctionsCallFail");
                }
            }
        }
    }
}
