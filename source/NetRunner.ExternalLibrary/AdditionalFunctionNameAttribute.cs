using System;

namespace NetRunner.ExternalLibrary
{
    /// <summary>
    /// Add function synonym for function to have different name in the fintesse test execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AdditionalFunctionNameAttribute : BaseOperationMarkerAttribute
    {
        public AdditionalFunctionNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(string.Format("Unable to set additional name '{0}' because it is empty", name), "name");
            }

            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
