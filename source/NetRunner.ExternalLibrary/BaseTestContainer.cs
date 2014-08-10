using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Base class for all test containers. Each type inherited this will be used for the functions lookup.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class BaseTestContainer : FunctionContainer
    {
    }
}
