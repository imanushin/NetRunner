using System;
using System.Collections.Generic;
using System.Text;
using NetRunner.Executable.Common;

namespace NetRunner.Executable.Invokation.Functions
{
    internal sealed class TestFunctionsSequence : AbstractTestFunction
    {
        public TestFunctionsSequence(IReadOnlyCollection<AbstractTestFunction> innerFunctions)
        {
            Validate.CollectionArgumentHasElements(innerFunctions, "innerFunctions");

            InnerFunctions = innerFunctions.ToReadOnlyList();
        }

        public ReadOnlyList<AbstractTestFunction> InnerFunctions
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            return InnerFunctions;
        }

        public override FunctionExecutionResult Invoke()
        {
            var results = new List<FunctionExecutionResult.FunctionRunResult>();
            var changes = new List<AbstractTableChange>();

            foreach (var innerFunction in InnerFunctions)
            {
                var localResult = innerFunction.Invoke();

                changes.AddRange(localResult.TableChanges);
                
                results.Add(localResult.ResultType);
            }

            var finalResult = FunctionExecutionResult.FunctionRunResult.Ignore;

            if (results.Contains(FunctionExecutionResult.FunctionRunResult.Exception))
            {
                finalResult = FunctionExecutionResult.FunctionRunResult.Exception;
            }
            else if (results.Contains(FunctionExecutionResult.FunctionRunResult.Fail))
            {
                finalResult = FunctionExecutionResult.FunctionRunResult.Fail;
            }
            else if (results.Contains(FunctionExecutionResult.FunctionRunResult.Success))
            {
                finalResult = FunctionExecutionResult.FunctionRunResult.Success;
            }

            return new FunctionExecutionResult(finalResult, changes);
        }
    }
}
