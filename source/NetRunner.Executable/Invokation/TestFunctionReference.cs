using System;
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

        public TestFunctionReference(MethodInfo method, BaseTestContainer testContainer)
        {
            this.method = method;
            TestContainer = testContainer;

            Name = method.Name;
            ArgumentTypes = method.GetParameters().Select(p => p.ParameterType).ToReadOnlyList();
        }

        public BaseTestContainer TestContainer
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public ReadOnlyList<Type> ArgumentTypes
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Name;
            yield return ArgumentTypes;
            yield return TestContainer;
        }

        protected override string GetString()
        {
            return string.Format("Method: {0}; target object type: {1}; Argument types: {2}", Name, TestContainer.GetType().Name, ArgumentTypes);
        }

        public object Invoke(object[] parameters)
        {
            return method.Invoke(TestContainer, parameters);
        }
    }
}
