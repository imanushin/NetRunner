
// ReSharper disable RedundantUsingDirective

using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetRunner.Executable.Common;
using NetRunner.Executable.Invokation;
using NetRunner.Executable.Invokation.Functions;
using NetRunner.Executable.Invokation.Keywords;
using NetRunner.Executable.RawData;
using NetRunner.Executable.Tests;
using NetRunner.Executable.Tests.Invokation;
using NetRunner.Executable.Tests.Invokation.Functions;
using NetRunner.Executable.Tests.Invokation.Keywords;
using NetRunner.Executable.Tests.RawData;
using NetRunner.ExternalLibrary;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper restore RedundantUsingDirective

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Global
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantExplicitArrayCreation
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantNameQualifier

#pragma warning disable 67
#pragma warning disable 219
#pragma warning disable 168


namespace NetRunner.Executable.Tests.Invokation.Functions
{

    [TestClass]
    public sealed partial class AbstractTableChangeTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AbstractTableChange> objects = new ObjectsCache<AbstractTableChange>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AbstractTableChange[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AbstractTableChangeTest.objects.Objects.Skip(1).ToArray(),
                AbstractTableChangeTest.objects.Objects.Take(2).ToArray(),
                AbstractTableChangeTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTableChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTableChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTableChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTableChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTableChange>GetApparents()
        {
            return
            new AbstractTableChange[0].Union(
AddCellExpandableInfoTest.objects).Union(
AddCellExpandableExceptionTest.objects).Union(
AddExceptionLineTest.objects).Union(
AddTraceLineTest.objects).Union(
AppendRowWithCellsTest.objects).Union(
CssClassCellChangeTest.objects).Union(
AddRowCssClassTest.objects).Union(
ExecutionFailedMessageTest.objects).Union(
ShowActualValueCellChangeTest.objects);
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTableChange> GetInstancesOfCurrentType()
        {
            return ReadOnlyList<AbstractTableChange>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTableChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTableChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTableChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddCellExpandableInfoTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AddCellExpandableInfo> objects = new ObjectsCache<AddCellExpandableInfo>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AddCellExpandableInfo[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AddCellExpandableInfoTest.objects.Objects.Skip(1).ToArray(),
                AddCellExpandableInfoTest.objects.Objects.Take(2).ToArray(),
                AddCellExpandableInfoTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableInfo First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableInfo Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableInfo Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddCellExpandableInfo> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddCellExpandableInfo>GetApparents()
        {
            return
            new AddCellExpandableInfo[0].Union(
AddCellExpandableExceptionTest.objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_CheckNullArg_cell()
        {
            var cell = HtmlCellTest.First;
            var header = "text 135346";
            var info = "text 135347";

            try
            {
                new AddCellExpandableInfo(null, header, info);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cell", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cell' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddCellExpandableInfo_CheckEmptyStringArg_header()
        {
            CheckEmptyStringArg_header(string.Empty);
            CheckEmptyStringArg_header("    ");
            CheckEmptyStringArg_header(Environment.NewLine);
            CheckEmptyStringArg_header("\n\r");
        }

        private void CheckEmptyStringArg_header(string stringArgument)
        {
            var cell = HtmlCellTest.First;
            var header = "text 135346";
            var info = "text 135347";

            try
            {
                new AddCellExpandableInfo(cell, stringArgument, info);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'header' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_CheckNullArg_header()
        {
            var cell = HtmlCellTest.First;
            var header = "text 135346";
            var info = "text 135347";

            try
            {
                new AddCellExpandableInfo(cell, null, info);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddCellExpandableInfo_CheckEmptyStringArg_info()
        {
            CheckEmptyStringArg_info(string.Empty);
            CheckEmptyStringArg_info("    ");
            CheckEmptyStringArg_info(Environment.NewLine);
            CheckEmptyStringArg_info("\n\r");
        }

        private void CheckEmptyStringArg_info(string stringArgument)
        {
            var cell = HtmlCellTest.First;
            var header = "text 135346";
            var info = "text 135347";

            try
            {
                new AddCellExpandableInfo(cell, header, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "info", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'info' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_CheckNullArg_info()
        {
            var cell = HtmlCellTest.First;
            var header = "text 135346";
            var info = "text 135347";

            try
            {
                new AddCellExpandableInfo(cell, header, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "info", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'info' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableInfo_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddCellExpandableExceptionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AddCellExpandableException> objects = new ObjectsCache<AddCellExpandableException>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AddCellExpandableException[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AddCellExpandableExceptionTest.objects.Objects.Skip(1).ToArray(),
                AddCellExpandableExceptionTest.objects.Objects.Take(2).ToArray(),
                AddCellExpandableExceptionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableException First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableException Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddCellExpandableException Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddCellExpandableException> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddCellExpandableException>GetApparents()
        {
            return ReadOnlyList<AddCellExpandableException>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_CheckNullArg_cell()
        {
            var cell = HtmlCellTest.First;
            var exception = new Exception("Text exception 135348");
            var headerFormat = "text 135349";
            var args = new Object[] { new object(), new object() };

            try
            {
                new AddCellExpandableException(null, exception, headerFormat, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cell", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cell' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_CheckNullArg_exception()
        {
            var cell = HtmlCellTest.First;
            var exception = new Exception("Text exception 135348");
            var headerFormat = "text 135349";
            var args = new Object[] { new object(), new object() };

            try
            {
                new AddCellExpandableException(cell, null, headerFormat, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exception", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exception' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddCellExpandableException_CheckEmptyStringArg_headerFormat()
        {
            CheckEmptyStringArg_headerFormat(string.Empty);
            CheckEmptyStringArg_headerFormat("    ");
            CheckEmptyStringArg_headerFormat(Environment.NewLine);
            CheckEmptyStringArg_headerFormat("\n\r");
        }

        private void CheckEmptyStringArg_headerFormat(string stringArgument)
        {
            var cell = HtmlCellTest.First;
            var exception = new Exception("Text exception 135348");
            var headerFormat = "text 135349";
            var args = new Object[] { new object(), new object() };

            try
            {
                new AddCellExpandableException(cell, exception, stringArgument, args);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "headerFormat", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'headerFormat' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_CheckNullArg_headerFormat()
        {
            var cell = HtmlCellTest.First;
            var exception = new Exception("Text exception 135348");
            var headerFormat = "text 135349";
            var args = new Object[] { new object(), new object() };

            try
            {
                new AddCellExpandableException(cell, exception, null, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "headerFormat", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'headerFormat' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_CheckNullArg_args()
        {
            var cell = HtmlCellTest.First;
            var exception = new Exception("Text exception 135348");
            var headerFormat = "text 135349";
            var args = new Object[] { new object(), new object() };

            try
            {
                new AddCellExpandableException(cell, exception, headerFormat, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "args", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'args' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddCellExpandableException_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AbstractTestFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AbstractTestFunction> objects = new ObjectsCache<AbstractTestFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AbstractTestFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AbstractTestFunctionTest.objects.Objects.Skip(1).ToArray(),
                AbstractTestFunctionTest.objects.Objects.Take(2).ToArray(),
                AbstractTestFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTestFunction>GetApparents()
        {
            return
            new AbstractTestFunction[0].Union(
BaseComplexArgumentedFunctionTest.objects).Union(
CollectionResultFunctionTest.objects).Union(
ProblematicFunctionTest.objects).Union(
TableResultFunctionTest.objects).Union(
EmptyTestFunctionTest.objects).Union(
TestFunctionsSequenceTest.objects).Union(
SimpleTestFunctionTest.objects);
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractTestFunction> GetInstancesOfCurrentType()
        {
            return ReadOnlyList<AbstractTestFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractTestFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseComplexArgumentedFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<BaseComplexArgumentedFunction> objects = new ObjectsCache<BaseComplexArgumentedFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<BaseComplexArgumentedFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                BaseComplexArgumentedFunctionTest.objects.Objects.Skip(1).ToArray(),
                BaseComplexArgumentedFunctionTest.objects.Objects.Take(2).ToArray(),
                BaseComplexArgumentedFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static BaseComplexArgumentedFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static BaseComplexArgumentedFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static BaseComplexArgumentedFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<BaseComplexArgumentedFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<BaseComplexArgumentedFunction>GetApparents()
        {
            return
            new BaseComplexArgumentedFunction[0].Union(
CollectionResultFunctionTest.objects).Union(
TableResultFunctionTest.objects);
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<BaseComplexArgumentedFunction> GetInstancesOfCurrentType()
        {
            return ReadOnlyList<BaseComplexArgumentedFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void BaseComplexArgumentedFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void BaseComplexArgumentedFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void BaseComplexArgumentedFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CollectionResultFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<CollectionResultFunction> objects = new ObjectsCache<CollectionResultFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<CollectionResultFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                CollectionResultFunctionTest.objects.Objects.Skip(1).ToArray(),
                CollectionResultFunctionTest.objects.Objects.Take(2).ToArray(),
                CollectionResultFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CollectionResultFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CollectionResultFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CollectionResultFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CollectionResultFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CollectionResultFunction>GetApparents()
        {
            return ReadOnlyList<CollectionResultFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_CheckNullArg_columnsRow()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionResultFunction(null, rows, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "columnsRow", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'columnsRow' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_CheckNullArg_rows()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionResultFunction(columnsRow, null, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rows", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rows' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_CheckNullArg_function()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionResultFunction(columnsRow, rows, null, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "function", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'function' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_CheckNullArg_functionToExecute()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionResultFunction(columnsRow, rows, function, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionToExecute", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionToExecute' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CollectionResultFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ProblematicFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<ProblematicFunction> objects = new ObjectsCache<ProblematicFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<ProblematicFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                ProblematicFunctionTest.objects.Objects.Skip(1).ToArray(),
                ProblematicFunctionTest.objects.Objects.Take(2).ToArray(),
                ProblematicFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ProblematicFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ProblematicFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ProblematicFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ProblematicFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ProblematicFunction>GetApparents()
        {
            return ReadOnlyList<ProblematicFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ProblematicFunction_CheckNullArg_tableChanges()
        {
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();

            try
            {
                new ProblematicFunction(null, rows);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableChanges", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableChanges' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ProblematicFunction_CheckNullArg_rows()
        {
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();

            try
            {
                new ProblematicFunction(tableChanges, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rows", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rows' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ProblematicFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ProblematicFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ProblematicFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class TableResultFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<TableResultFunction> objects = new ObjectsCache<TableResultFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<TableResultFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                TableResultFunctionTest.objects.Objects.Skip(1).ToArray(),
                TableResultFunctionTest.objects.Objects.Take(2).ToArray(),
                TableResultFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TableResultFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TableResultFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TableResultFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TableResultFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TableResultFunction>GetApparents()
        {
            return ReadOnlyList<TableResultFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_CheckNullArg_columnsRow()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new TableResultFunction(null, rows, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "columnsRow", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'columnsRow' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_CheckNullArg_rows()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new TableResultFunction(columnsRow, null, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rows", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rows' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_CheckNullArg_function()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new TableResultFunction(columnsRow, rows, null, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "function", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'function' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_CheckNullArg_functionToExecute()
        {
            var columnsRow = HtmlRowTest.First;
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new TableResultFunction(columnsRow, rows, function, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionToExecute", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionToExecute' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TableResultFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddExceptionLineTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AddExceptionLine> objects = new ObjectsCache<AddExceptionLine>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AddExceptionLine[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AddExceptionLineTest.objects.Objects.Skip(1).ToArray(),
                AddExceptionLineTest.objects.Objects.Take(2).ToArray(),
                AddExceptionLineTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddExceptionLine First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddExceptionLine Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddExceptionLine Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddExceptionLine> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddExceptionLine>GetApparents()
        {
            return ReadOnlyList<AddExceptionLine>.Empty;
        }

        [TestMethod]
        public void AddExceptionLine_CheckEmptyStringArg_header_0()
        {
            CheckEmptyStringArg_header_0(string.Empty);
            CheckEmptyStringArg_header_0("    ");
            CheckEmptyStringArg_header_0(Environment.NewLine);
            CheckEmptyStringArg_header_0("\n\r");
        }

        private void CheckEmptyStringArg_header_0(string stringArgument)
        {
            var header = "text 135350";
            var exception = new Exception("Text exception 135351");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(stringArgument, exception, rowReference);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'header' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_header_135352()
        {
            var header = "text 135350";
            var exception = new Exception("Text exception 135351");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine((String)null, exception, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_exception_135353()
        {
            var header = "text 135350";
            var exception = new Exception("Text exception 135351");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(header, (Exception)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exception", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exception' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_rowReference_135354()
        {
            var header = "text 135350";
            var exception = new Exception("Text exception 135351");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(header, exception, (HtmlRowReference)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddExceptionLine_CheckEmptyStringArg_header_1()
        {
            CheckEmptyStringArg_header_1(string.Empty);
            CheckEmptyStringArg_header_1("    ");
            CheckEmptyStringArg_header_1(Environment.NewLine);
            CheckEmptyStringArg_header_1("\n\r");
        }

        private void CheckEmptyStringArg_header_1(string stringArgument)
        {
            var header = "text 135355";
            var exceptions = new List<Exception>{ new Exception("Text exception 135356") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(stringArgument, exceptions, rowReference);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'header' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_header_135357()
        {
            var header = "text 135355";
            var exceptions = new List<Exception>{ new Exception("Text exception 135356") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine((String)null, exceptions, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_exceptions_135358()
        {
            var header = "text 135355";
            var exceptions = new List<Exception>{ new Exception("Text exception 135356") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(header, (IReadOnlyCollection<Exception>)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exceptions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exceptions' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_rowReference_135359()
        {
            var header = "text 135355";
            var exceptions = new List<Exception>{ new Exception("Text exception 135356") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(header, exceptions, (HtmlRowReference)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_exception_135361()
        {
            var exception = new Exception("Text exception 135360");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine((Exception)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exception", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exception' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_rowReference_135362()
        {
            var exception = new Exception("Text exception 135360");
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(exception, (HtmlRowReference)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_exceptions_135364()
        {
            var exceptions = new List<Exception>{ new Exception("Text exception 135363") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine((IReadOnlyCollection<Exception>)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exceptions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exceptions' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_CheckNullArg_rowReference_135365()
        {
            var exceptions = new List<Exception>{ new Exception("Text exception 135363") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(exceptions, (HtmlRowReference)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddExceptionLine_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddTraceLineTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AddTraceLine> objects = new ObjectsCache<AddTraceLine>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AddTraceLine[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AddTraceLineTest.objects.Objects.Skip(1).ToArray(),
                AddTraceLineTest.objects.Objects.Take(2).ToArray(),
                AddTraceLineTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddTraceLine First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddTraceLine Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddTraceLine Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddTraceLine> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddTraceLine>GetApparents()
        {
            return ReadOnlyList<AddTraceLine>.Empty;
        }

        [TestMethod]
        public void AddTraceLine_CheckEmptyStringArg_text()
        {
            CheckEmptyStringArg_text(string.Empty);
            CheckEmptyStringArg_text("    ");
            CheckEmptyStringArg_text(Environment.NewLine);
            CheckEmptyStringArg_text("\n\r");
        }

        private void CheckEmptyStringArg_text(string stringArgument)
        {
            var text = "text 135366";
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddTraceLine(stringArgument, rowReference);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "text", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'text' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddTraceLine_CheckNullArg_text()
        {
            var text = "text 135366";
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddTraceLine(null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "text", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'text' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddTraceLine_CheckNullArg_rowReference()
        {
            var text = "text 135366";
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddTraceLine(text, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddTraceLine_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddTraceLine_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddTraceLine_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AppendRowWithCellsTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AppendRowWithCells> objects = new ObjectsCache<AppendRowWithCells>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AppendRowWithCells[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AppendRowWithCellsTest.objects.Objects.Skip(1).ToArray(),
                AppendRowWithCellsTest.objects.Objects.Take(2).ToArray(),
                AppendRowWithCellsTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AppendRowWithCells First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AppendRowWithCells Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AppendRowWithCells Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AppendRowWithCells> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AppendRowWithCells>GetApparents()
        {
            return ReadOnlyList<AppendRowWithCells>.Empty;
        }

        [TestMethod]
        public void AppendRowWithCells_CheckEmptyStringArg_cellClass()
        {
            CheckEmptyStringArg_cellClass(string.Empty);
            CheckEmptyStringArg_cellClass("    ");
            CheckEmptyStringArg_cellClass(Environment.NewLine);
            CheckEmptyStringArg_cellClass("\n\r");
        }

        private void CheckEmptyStringArg_cellClass(string stringArgument)
        {
            var cellClass = "text 135367";
            var cellHtmlDatas = new List<String>{ "text 135368" }.ToReadOnlyList();

            try
            {
                new AppendRowWithCells(stringArgument, cellHtmlDatas);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "cellClass", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'cellClass' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AppendRowWithCells_CheckNullArg_cellClass()
        {
            var cellClass = "text 135367";
            var cellHtmlDatas = new List<String>{ "text 135368" }.ToReadOnlyList();

            try
            {
                new AppendRowWithCells(null, cellHtmlDatas);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cellClass", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cellClass' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AppendRowWithCells_CheckNullArg_cellHtmlDatas()
        {
            var cellClass = "text 135367";
            var cellHtmlDatas = new List<String>{ "text 135368" }.ToReadOnlyList();

            try
            {
                new AppendRowWithCells(cellClass, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cellHtmlDatas", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cellHtmlDatas' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AppendRowWithCells_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AppendRowWithCells_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AppendRowWithCells_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CssClassCellChangeTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<CssClassCellChange> objects = new ObjectsCache<CssClassCellChange>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<CssClassCellChange[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                CssClassCellChangeTest.objects.Objects.Skip(1).ToArray(),
                CssClassCellChangeTest.objects.Objects.Take(2).ToArray(),
                CssClassCellChangeTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CssClassCellChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CssClassCellChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CssClassCellChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CssClassCellChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CssClassCellChange>GetApparents()
        {
            return
            new CssClassCellChange[0].Union(
ShowActualValueCellChangeTest.objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CssClassCellChange_CheckNullArg_cell()
        {
            var cell = HtmlCellTest.First;
            var newClass = "text 135369";

            try
            {
                new CssClassCellChange(null, newClass);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cell", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cell' isn't checked for null inputs");
        }

        [TestMethod]
        public void CssClassCellChange_CheckEmptyStringArg_newClass()
        {
            CheckEmptyStringArg_newClass(string.Empty);
            CheckEmptyStringArg_newClass("    ");
            CheckEmptyStringArg_newClass(Environment.NewLine);
            CheckEmptyStringArg_newClass("\n\r");
        }

        private void CheckEmptyStringArg_newClass(string stringArgument)
        {
            var cell = HtmlCellTest.First;
            var newClass = "text 135369";

            try
            {
                new CssClassCellChange(cell, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "newClass", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'newClass' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CssClassCellChange_CheckNullArg_newClass()
        {
            var cell = HtmlCellTest.First;
            var newClass = "text 135369";

            try
            {
                new CssClassCellChange(cell, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "newClass", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'newClass' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CssClassCellChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CssClassCellChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CssClassCellChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class EmptyTestFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<EmptyTestFunction> objects = new ObjectsCache<EmptyTestFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<EmptyTestFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                EmptyTestFunctionTest.objects.Objects.Skip(1).ToArray(),
                EmptyTestFunctionTest.objects.Objects.Take(2).ToArray(),
                EmptyTestFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<EmptyTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<EmptyTestFunction>GetApparents()
        {
            return ReadOnlyList<EmptyTestFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyTestFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddRowCssClassTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AddRowCssClass> objects = new ObjectsCache<AddRowCssClass>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AddRowCssClass[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AddRowCssClassTest.objects.Objects.Skip(1).ToArray(),
                AddRowCssClassTest.objects.Objects.Take(2).ToArray(),
                AddRowCssClassTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddRowCssClass First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddRowCssClass Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AddRowCssClass Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddRowCssClass> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AddRowCssClass>GetApparents()
        {
            return ReadOnlyList<AddRowCssClass>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddRowCssClass_CheckNullArg_rowReference()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var targetCssClass = "text 135370";

            try
            {
                new AddRowCssClass(null, targetCssClass);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddRowCssClass_CheckEmptyStringArg_targetCssClass()
        {
            CheckEmptyStringArg_targetCssClass(string.Empty);
            CheckEmptyStringArg_targetCssClass("    ");
            CheckEmptyStringArg_targetCssClass(Environment.NewLine);
            CheckEmptyStringArg_targetCssClass("\n\r");
        }

        private void CheckEmptyStringArg_targetCssClass(string stringArgument)
        {
            var rowReference = HtmlRowReferenceTest.First;
            var targetCssClass = "text 135370";

            try
            {
                new AddRowCssClass(rowReference, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "targetCssClass", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'targetCssClass' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddRowCssClass_CheckNullArg_targetCssClass()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var targetCssClass = "text 135370";

            try
            {
                new AddRowCssClass(rowReference, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetCssClass", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetCssClass' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddRowCssClass_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddRowCssClass_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AddRowCssClass_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ExecutionFailedMessageTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<ExecutionFailedMessage> objects = new ObjectsCache<ExecutionFailedMessage>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<ExecutionFailedMessage[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                ExecutionFailedMessageTest.objects.Objects.Skip(1).ToArray(),
                ExecutionFailedMessageTest.objects.Objects.Take(2).ToArray(),
                ExecutionFailedMessageTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ExecutionFailedMessage First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ExecutionFailedMessage Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ExecutionFailedMessage Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ExecutionFailedMessage> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ExecutionFailedMessage>GetApparents()
        {
            return ReadOnlyList<ExecutionFailedMessage>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_CheckNullArg_rowReference()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(null, header, messageFormat, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        public void ExecutionFailedMessage_CheckEmptyStringArg_header()
        {
            CheckEmptyStringArg_header(string.Empty);
            CheckEmptyStringArg_header("    ");
            CheckEmptyStringArg_header(Environment.NewLine);
            CheckEmptyStringArg_header("\n\r");
        }

        private void CheckEmptyStringArg_header(string stringArgument)
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(rowReference, stringArgument, messageFormat, args);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'header' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_CheckNullArg_header()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(rowReference, null, messageFormat, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        public void ExecutionFailedMessage_CheckEmptyStringArg_messageFormat()
        {
            CheckEmptyStringArg_messageFormat(string.Empty);
            CheckEmptyStringArg_messageFormat("    ");
            CheckEmptyStringArg_messageFormat(Environment.NewLine);
            CheckEmptyStringArg_messageFormat("\n\r");
        }

        private void CheckEmptyStringArg_messageFormat(string stringArgument)
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(rowReference, header, stringArgument, args);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "messageFormat", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'messageFormat' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_CheckNullArg_messageFormat()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(rowReference, header, null, args);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "messageFormat", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'messageFormat' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_CheckNullArg_args()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135371";
            var messageFormat = "text 135372";
            var args = new Object[] { new object(), new object() };

            try
            {
                new ExecutionFailedMessage(rowReference, header, messageFormat, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "args", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'args' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ExecutionFailedMessage_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ShowActualValueCellChangeTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<ShowActualValueCellChange> objects = new ObjectsCache<ShowActualValueCellChange>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<ShowActualValueCellChange[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                ShowActualValueCellChangeTest.objects.Objects.Skip(1).ToArray(),
                ShowActualValueCellChangeTest.objects.Objects.Take(2).ToArray(),
                ShowActualValueCellChangeTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ShowActualValueCellChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ShowActualValueCellChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static ShowActualValueCellChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ShowActualValueCellChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<ShowActualValueCellChange>GetApparents()
        {
            return ReadOnlyList<ShowActualValueCellChange>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ShowActualValueCellChange_CheckNullArg_cell()
        {
            var cell = HtmlCellTest.First;
            var actualValue = new object();

            try
            {
                new ShowActualValueCellChange(null, actualValue);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cell", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cell' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ShowActualValueCellChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ShowActualValueCellChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void ShowActualValueCellChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class TestFunctionsSequenceTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<TestFunctionsSequence> objects = new ObjectsCache<TestFunctionsSequence>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<TestFunctionsSequence[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                TestFunctionsSequenceTest.objects.Objects.Skip(1).ToArray(),
                TestFunctionsSequenceTest.objects.Objects.Take(2).ToArray(),
                TestFunctionsSequenceTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionsSequence First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionsSequence Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionsSequence Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TestFunctionsSequence> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TestFunctionsSequence>GetApparents()
        {
            return ReadOnlyList<TestFunctionsSequence>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionsSequence_CheckNullArg_innerFunctions()
        {
            var innerFunctions = new List<AbstractTestFunction>{ AbstractTestFunctionTest.First }.ToReadOnlyList();

            try
            {
                new TestFunctionsSequence(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "innerFunctions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'innerFunctions' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionsSequence_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionsSequence_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionsSequence_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class SimpleTestFunctionTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<SimpleTestFunction> objects = new ObjectsCache<SimpleTestFunction>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<SimpleTestFunction[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                SimpleTestFunctionTest.objects.Objects.Skip(1).ToArray(),
                SimpleTestFunctionTest.objects.Objects.Take(2).ToArray(),
                SimpleTestFunctionTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static SimpleTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static SimpleTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static SimpleTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<SimpleTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<SimpleTestFunction>GetApparents()
        {
            return ReadOnlyList<SimpleTestFunction>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_CheckNullArg_header()
        {
            var header = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;
            var targetRow = HtmlRowTest.First;

            try
            {
                new SimpleTestFunction(null, functionToExecute, targetRow);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_CheckNullArg_functionToExecute()
        {
            var header = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;
            var targetRow = HtmlRowTest.First;

            try
            {
                new SimpleTestFunction(header, null, targetRow);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionToExecute", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionToExecute' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_CheckNullArg_targetRow()
        {
            var header = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;
            var targetRow = HtmlRowTest.First;

            try
            {
                new SimpleTestFunction(header, functionToExecute, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetRow", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetRow' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void SimpleTestFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace NetRunner.Executable.Tests.Invokation.Keywords
{

    [TestClass]
    public sealed partial class AbstractKeywordTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<AbstractKeyword> objects = new ObjectsCache<AbstractKeyword>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<AbstractKeyword[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                AbstractKeywordTest.objects.Objects.Skip(1).ToArray(),
                AbstractKeywordTest.objects.Objects.Take(2).ToArray(),
                AbstractKeywordTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static AbstractKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractKeyword>GetApparents()
        {
            return
            new AbstractKeyword[0].Union(
CheckResultKeywordTest.objects).Union(
EmptyKeywordTest.objects).Union(
UnknownKeywordTest.objects);
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<AbstractKeyword> GetInstancesOfCurrentType()
        {
            return ReadOnlyList<AbstractKeyword>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void AbstractKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CheckResultKeywordTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<CheckResultKeyword> objects = new ObjectsCache<CheckResultKeyword>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<CheckResultKeyword[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                CheckResultKeywordTest.objects.Objects.Skip(1).ToArray(),
                CheckResultKeywordTest.objects.Objects.Take(2).ToArray(),
                CheckResultKeywordTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CheckResultKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CheckResultKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static CheckResultKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CheckResultKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<CheckResultKeyword>GetApparents()
        {
            return ReadOnlyList<CheckResultKeyword>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CheckResultKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CheckResultKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void CheckResultKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class EmptyKeywordTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<EmptyKeyword> objects = new ObjectsCache<EmptyKeyword>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<EmptyKeyword[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                EmptyKeywordTest.objects.Objects.Skip(1).ToArray(),
                EmptyKeywordTest.objects.Objects.Take(2).ToArray(),
                EmptyKeywordTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static EmptyKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<EmptyKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<EmptyKeyword>GetApparents()
        {
            return ReadOnlyList<EmptyKeyword>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyKeyword_CheckNullArg_cells()
        {
            var cells = new List<HtmlCell>{ HtmlCellTest.First }.ToReadOnlyList();

            try
            {
                new EmptyKeyword(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cells", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cells' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void EmptyKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class UnknownKeywordTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<UnknownKeyword> objects = new ObjectsCache<UnknownKeyword>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<UnknownKeyword[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                UnknownKeywordTest.objects.Objects.Skip(1).ToArray(),
                UnknownKeywordTest.objects.Objects.Take(2).ToArray(),
                UnknownKeywordTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static UnknownKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static UnknownKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static UnknownKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<UnknownKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<UnknownKeyword>GetApparents()
        {
            return ReadOnlyList<UnknownKeyword>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void UnknownKeyword_CheckNullArg_cells()
        {
            var cells = new List<HtmlCell>{ HtmlCellTest.First }.ToReadOnlyList();

            try
            {
                new UnknownKeyword(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cells", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cells' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void UnknownKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void UnknownKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void UnknownKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace NetRunner.Executable.Tests.Invokation
{

    [TestClass]
    public sealed partial class FunctionExecutionResultTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<FunctionExecutionResult> objects = new ObjectsCache<FunctionExecutionResult>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<FunctionExecutionResult[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                FunctionExecutionResultTest.objects.Objects.Skip(1).ToArray(),
                FunctionExecutionResultTest.objects.Objects.Take(2).ToArray(),
                FunctionExecutionResultTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionExecutionResult First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionExecutionResult Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionExecutionResult Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FunctionExecutionResult> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FunctionExecutionResult>GetApparents()
        {
            return ReadOnlyList<FunctionExecutionResult>.Empty;
        }

        [TestMethod]
        public void FunctionExecutionResult_CheckWrongEnumArg_resultType_135373()
        {
            var resultType = Enum.GetValues(typeof(FunctionExecutionResult.FunctionRunResult)).Cast<FunctionExecutionResult.FunctionRunResult>().First();
            var tableChanges = new AbstractTableChange[] { AbstractTableChangeTest.First, AbstractTableChangeTest.First };

            try
            {
                new FunctionExecutionResult((FunctionExecutionResult.FunctionRunResult)4, tableChanges);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "resultType", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'resultType' isn't checked for the not-defined values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionExecutionResult_CheckNullArg_tableChanges_135374()
        {
            var resultType = Enum.GetValues(typeof(FunctionExecutionResult.FunctionRunResult)).Cast<FunctionExecutionResult.FunctionRunResult>().First();
            var tableChanges = new AbstractTableChange[] { AbstractTableChangeTest.First, AbstractTableChangeTest.First };

            try
            {
                new FunctionExecutionResult(resultType, (AbstractTableChange[])null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableChanges", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableChanges' isn't checked for null inputs");
        }

        [TestMethod]
        public void FunctionExecutionResult_CheckWrongEnumArg_resultType_135375()
        {
            var resultType = Enum.GetValues(typeof(FunctionExecutionResult.FunctionRunResult)).Cast<FunctionExecutionResult.FunctionRunResult>().First();
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();

            try
            {
                new FunctionExecutionResult((FunctionExecutionResult.FunctionRunResult)4, tableChanges);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "resultType", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'resultType' isn't checked for the not-defined values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionExecutionResult_CheckNullArg_tableChanges_135376()
        {
            var resultType = Enum.GetValues(typeof(FunctionExecutionResult.FunctionRunResult)).Cast<FunctionExecutionResult.FunctionRunResult>().First();
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();

            try
            {
                new FunctionExecutionResult(resultType, (IEnumerable<AbstractTableChange>)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableChanges", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableChanges' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionExecutionResult_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionExecutionResult_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionExecutionResult_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class FunctionHeaderTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<FunctionHeader> objects = new ObjectsCache<FunctionHeader>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<FunctionHeader[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                FunctionHeaderTest.objects.Objects.Skip(1).ToArray(),
                FunctionHeaderTest.objects.Objects.Take(2).ToArray(),
                FunctionHeaderTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionHeader First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionHeader Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FunctionHeader Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FunctionHeader> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FunctionHeader>GetApparents()
        {
            return ReadOnlyList<FunctionHeader>.Empty;
        }

        [TestMethod]
        public void FunctionHeader_CheckEmptyStringArg_functionName()
        {
            CheckEmptyStringArg_functionName(string.Empty);
            CheckEmptyStringArg_functionName("    ");
            CheckEmptyStringArg_functionName(Environment.NewLine);
            CheckEmptyStringArg_functionName("\n\r");
        }

        private void CheckEmptyStringArg_functionName(string stringArgument)
        {
            var functionName = "text 135377";
            var arguments = new List<String>{ "text 135378" }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;
            var keyword = AbstractKeywordTest.First;

            try
            {
                new FunctionHeader(stringArgument, arguments, rowReference, keyword);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "functionName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'functionName' isn't checked for emply values");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_CheckNullArg_functionName()
        {
            var functionName = "text 135377";
            var arguments = new List<String>{ "text 135378" }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;
            var keyword = AbstractKeywordTest.First;

            try
            {
                new FunctionHeader(null, arguments, rowReference, keyword);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionName' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_CheckNullArg_arguments()
        {
            var functionName = "text 135377";
            var arguments = new List<String>{ "text 135378" }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;
            var keyword = AbstractKeywordTest.First;

            try
            {
                new FunctionHeader(functionName, null, rowReference, keyword);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "arguments", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'arguments' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_CheckNullArg_rowReference()
        {
            var functionName = "text 135377";
            var arguments = new List<String>{ "text 135378" }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;
            var keyword = AbstractKeywordTest.First;

            try
            {
                new FunctionHeader(functionName, arguments, null, keyword);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_CheckNullArg_keyword()
        {
            var functionName = "text 135377";
            var arguments = new List<String>{ "text 135378" }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;
            var keyword = AbstractKeywordTest.First;

            try
            {
                new FunctionHeader(functionName, arguments, rowReference, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "keyword", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'keyword' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FunctionHeader_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlRowReferenceTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<HtmlRowReference> objects = new ObjectsCache<HtmlRowReference>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<HtmlRowReference[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                HtmlRowReferenceTest.objects.Objects.Skip(1).ToArray(),
                HtmlRowReferenceTest.objects.Objects.Take(2).ToArray(),
                HtmlRowReferenceTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRowReference First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRowReference Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRowReference Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlRowReference> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlRowReference>GetApparents()
        {
            return ReadOnlyList<HtmlRowReference>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRowReference_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRowReference_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRowReference_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class TestFunctionReferenceTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<TestFunctionReference> objects = new ObjectsCache<TestFunctionReference>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<TestFunctionReference[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                TestFunctionReferenceTest.objects.Objects.Skip(1).ToArray(),
                TestFunctionReferenceTest.objects.Objects.Take(2).ToArray(),
                TestFunctionReferenceTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionReference First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionReference Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static TestFunctionReference Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TestFunctionReference> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<TestFunctionReference>GetApparents()
        {
            return ReadOnlyList<TestFunctionReference>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionReference_CheckNullArg_method()
        {
            var method = GetType().GetMethods().First();
            var targetObject = new FakeFunctionContainer(1);

            try
            {
                new TestFunctionReference(null, targetObject);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "method", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'method' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionReference_CheckNullArg_targetObject()
        {
            var method = GetType().GetMethods().First();
            var targetObject = new FakeFunctionContainer(1);

            try
            {
                new TestFunctionReference(method, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetObject", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetObject' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionReference_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionReference_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void TestFunctionReference_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace NetRunner.Executable.Tests.RawData
{

    [TestClass]
    public sealed partial class FitnesseHtmlDocumentTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<FitnesseHtmlDocument> objects = new ObjectsCache<FitnesseHtmlDocument>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<FitnesseHtmlDocument[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                FitnesseHtmlDocumentTest.objects.Objects.Skip(1).ToArray(),
                FitnesseHtmlDocumentTest.objects.Objects.Take(2).ToArray(),
                FitnesseHtmlDocumentTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FitnesseHtmlDocument First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FitnesseHtmlDocument Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static FitnesseHtmlDocument Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FitnesseHtmlDocument> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<FitnesseHtmlDocument>GetApparents()
        {
            return ReadOnlyList<FitnesseHtmlDocument>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FitnesseHtmlDocument_CheckNullArg_textBeforeFirstTable()
        {
            var textBeforeFirstTable = "text 135379";
            var tables = new List<HtmlTable>{ HtmlTableTest.First }.ToReadOnlyList();

            try
            {
                new FitnesseHtmlDocument(null, tables);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "textBeforeFirstTable", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'textBeforeFirstTable' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FitnesseHtmlDocument_CheckNullArg_tables()
        {
            var textBeforeFirstTable = "text 135379";
            var tables = new List<HtmlTable>{ HtmlTableTest.First }.ToReadOnlyList();

            try
            {
                new FitnesseHtmlDocument(textBeforeFirstTable, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tables", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tables' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FitnesseHtmlDocument_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FitnesseHtmlDocument_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void FitnesseHtmlDocument_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlCellTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<HtmlCell> objects = new ObjectsCache<HtmlCell>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<HtmlCell[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                HtmlCellTest.objects.Objects.Skip(1).ToArray(),
                HtmlCellTest.objects.Objects.Take(2).ToArray(),
                HtmlCellTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlCell First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlCell Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlCell Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlCell> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlCell>GetApparents()
        {
            return ReadOnlyList<HtmlCell>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlCell_CheckNullArg_tableCell()
        {
            var tableCell = HtmlNode.CreateNode("<i>TEST<i/>");

            try
            {
                new HtmlCell(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableCell", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableCell' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlCell_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlCell_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlCell_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlRowTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<HtmlRow> objects = new ObjectsCache<HtmlRow>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<HtmlRow[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                HtmlRowTest.objects.Objects.Skip(1).ToArray(),
                HtmlRowTest.objects.Objects.Take(2).ToArray(),
                HtmlRowTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRow First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRow Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlRow Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlRow> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlRow>GetApparents()
        {
            return ReadOnlyList<HtmlRow>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRow_CheckNullArg_cellsEntries()
        {
            var cellsEntries = new List<HtmlCell>{ HtmlCellTest.First }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new HtmlRow(null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "cellsEntries", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'cellsEntries' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRow_CheckNullArg_rowReference()
        {
            var cellsEntries = new List<HtmlCell>{ HtmlCellTest.First }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new HtmlRow(cellsEntries, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rowReference", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rowReference' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRow_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRow_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlRow_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlTableTest : ReadOnlyObjectTest
    {
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static readonly ObjectsCache<HtmlTable> objects = new ObjectsCache<HtmlTable>(GetInstances);

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static IEnumerable<HtmlTable[]> CreateNonEmptyObjectsArrays()
        {
            return new[]
            {
                HtmlTableTest.objects.Objects.Skip(1).ToArray(),
                HtmlTableTest.objects.Objects.Take(2).ToArray(),
                HtmlTableTest.objects.Objects.Take(1).ToArray()
            };
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlTable First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlTable Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        internal static HtmlTable Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlTable> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        [GeneratedCode("TestGenerator", "1.0.0.0")]
        private static IEnumerable<HtmlTable>GetApparents()
        {
            return ReadOnlyList<HtmlTable>.Empty;
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_CheckNullArg_rows()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = HtmlNode.CreateNode("<i>TEST<i/>");
            var textAfterTable = "text 135380";

            try
            {
                new HtmlTable(null, tableNode, textAfterTable);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rows", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rows' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_CheckNullArg_tableNode()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = HtmlNode.CreateNode("<i>TEST<i/>");
            var textAfterTable = "text 135380";

            try
            {
                new HtmlTable(rows, null, textAfterTable);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableNode", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableNode' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_CheckNullArg_textAfterTable()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = HtmlNode.CreateNode("<i>TEST<i/>");
            var textAfterTable = "text 135380";

            try
            {
                new HtmlTable(rows, tableNode, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "textAfterTable", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'textAfterTable' isn't checked for null inputs");
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        [GeneratedCode("TestGenerator", "1.0.0.0")]
        public void HtmlTable_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}


#pragma warning restore 219
#pragma warning restore 67
#pragma warning restore 168

// ReSharper restore RedundantNameQualifier
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper restore RedundantExplicitArrayCreation
// ReSharper restore PartialTypeWithSinglePart
// ReSharper restore RedundantCast
// ReSharper restore UnusedVariable
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper restore ObjectCreationAsStatement
// ReSharper restore ConvertToConstant.Local
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global

