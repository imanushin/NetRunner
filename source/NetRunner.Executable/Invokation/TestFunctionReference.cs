using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetRunner.Executable.Common;
using NetRunner.ExternalLibrary;

namespace NetRunner.Executable.Invokation
{
    internal sealed class TestFunctionReference : BaseReadOnlyObject
    {
        private readonly MethodInfo method;

        public TestFunctionReference(MethodInfo method, FunctionContainer targetObject)
        {
            this.method = method;
            TargetObject = targetObject;

            Name = method.Name;
            ArgumentTypes = method.GetParameters().ToReadOnlyList();
            ResultType = method.ReturnType;
        }

        public FunctionContainer TargetObject
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public ReadOnlyList<ParameterInfo> ArgumentTypes
        {
            get;
            private set;
        }

        public Type ResultType
        {
            get;
            private set;
        }

        public bool HasStrongResult
        {
            get
            {
                return typeof(IEnumerable).IsAssignableFrom(method.ReturnType)
                    || typeof(BaseTableArgument).IsAssignableFrom(method.ReturnType);
            }
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Name;
            yield return ArgumentTypes;
            yield return TargetObject;
            yield return ResultType;
        }

        protected override string GetString()
        {
            return string.Format("Method: {0}; target object type: {1}; Parameters: {2}; Result type: {3}", Name, TargetObject.GetType().Name, ArgumentTypes, ResultType);
        }

        public object Invoke(object[] parameters)
        {
            return method.Invoke(TargetObject, parameters);
        }
    }
}
