using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// System class for all types which are used from the FitNesse markup.
    /// </summary>
    public abstract class FunctionContainer
    {
        public delegate void BeforeFunctionCallHandler(MethodInfo targetFunction);
        public delegate void AfterFunctionCallHandler(MethodInfo targetFunction);

        public event BeforeFunctionCallHandler BeforeFunctionCall;
        public event AfterFunctionCallHandler AfterFunctionCall;

        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyAfterFunctionCall(MethodInfo targetFunction)
        {
            if (AfterFunctionCall == null)
                return null;

            try
            {
                AfterFunctionCall(targetFunction);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        [CanBeNull]
        [UsedImplicitly]
        internal Exception NotifyBeforeFunctionCall(MethodInfo targetFunction)
        {
            if (BeforeFunctionCall == null)
                return null;

            try
            {
                BeforeFunctionCall(targetFunction);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

    }
}
