using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public abstract class BaseTableArgument : FunctionContainer
    {
        public delegate void BeforeFunctionCallHandler(string expectedFunctionName);
        public delegate void AfterFunctionCallHandler(string functionCalledName);

        public event BeforeFunctionCallHandler BeforeFunctionCall;
        public event AfterFunctionCallHandler AfterFunctionCall;

        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyAfterFunctionCall(string functionCalledName)
        {
            if (AfterFunctionCall == null)
                return null;

            try
            {
                AfterFunctionCall(functionCalledName);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyBeforeFunctionCall(string expectedFunctionName)
        {
            if (BeforeFunctionCall == null)
                return null;

            try
            {
                BeforeFunctionCall(expectedFunctionName);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
