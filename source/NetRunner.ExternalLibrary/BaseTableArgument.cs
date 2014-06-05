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
        public delegate void AfterAllFunctionsCallHandler(MethodInfo method);
        public delegate void BeforeAllFunctionsCallHandler(MethodInfo method);

        public event AfterAllFunctionsCallHandler AfterAllFunctionsCall;
        public event BeforeAllFunctionsCallHandler BeforeAllFunctionsCall;

        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyAfterAllFunctionsCall(MethodInfo method)
        {
            if (AfterAllFunctionsCall == null)
                return null;

            try
            {
                AfterAllFunctionsCall(method);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyBeforeAllFunctionsCall(MethodInfo method)
        {
            if (BeforeAllFunctionsCall == null)
                return null;

            try
            {
                BeforeAllFunctionsCall(method);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

    }
}
