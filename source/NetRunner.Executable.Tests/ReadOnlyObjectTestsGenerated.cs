
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Global

#pragma warning disable 67
#pragma warning disable 219


namespace NetRunner.Executable.Tests.Invokation.Keywords
{

    [TestClass]
    public sealed partial class AbstractKeywordTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AbstractKeyword> objects = new ObjectsCache<AbstractKeyword>(GetInstances);

        internal static AbstractKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AbstractKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AbstractKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AbstractKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<AbstractKeyword>GetApparents()
        {
            return
            new AbstractKeyword[0].Union(
CheckResultKeywordTest.objects).Union(
EmptyKeywordTest.objects).Union(
UnknownKeywordTest.objects);
        }

        [TestMethod]
        public void AbstractKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AbstractKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AbstractKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CheckResultKeywordTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CheckResultKeyword> objects = new ObjectsCache<CheckResultKeyword>(GetInstances);

        internal static CheckResultKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CheckResultKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CheckResultKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<CheckResultKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<CheckResultKeyword>GetApparents()
        {
            return ReadOnlyList<CheckResultKeyword>.Empty;
        }

        [TestMethod]
        public void CheckResultKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CheckResultKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CheckResultKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class EmptyKeywordTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<EmptyKeyword> objects = new ObjectsCache<EmptyKeyword>(GetInstances);

        internal static EmptyKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static EmptyKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static EmptyKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<EmptyKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<EmptyKeyword>GetApparents()
        {
            return ReadOnlyList<EmptyKeyword>.Empty;
        }

        [TestMethod]
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
        public void EmptyKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void EmptyKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void EmptyKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class UnknownKeywordTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<UnknownKeyword> objects = new ObjectsCache<UnknownKeyword>(GetInstances);

        internal static UnknownKeyword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static UnknownKeyword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static UnknownKeyword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<UnknownKeyword> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<UnknownKeyword>GetApparents()
        {
            return ReadOnlyList<UnknownKeyword>.Empty;
        }

        [TestMethod]
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
        public void UnknownKeyword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void UnknownKeyword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void UnknownKeyword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace NetRunner.Executable.Tests.Invokation.Functions
{

    [TestClass]
    public sealed partial class AbstractTableChangeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AbstractTableChange> objects = new ObjectsCache<AbstractTableChange>(GetInstances);

        internal static AbstractTableChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AbstractTableChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AbstractTableChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AbstractTableChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<AbstractTableChange>GetApparents()
        {
            return
            new AbstractTableChange[0].Union(
AddCellExpandableInfoTest.objects).Union(
AddExceptionLineTest.objects).Union(
AddTraceLineTest.objects).Union(
AppendRowWithCellsTest.objects).Union(
CssClassCellChangeTest.objects).Union(
AddRowCssClassTest.objects).Union(
ExecutionFailedMessageTest.objects).Union(
ShowActualValueCellChangeTest.objects);
        }

        [TestMethod]
        public void AbstractTableChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AbstractTableChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AbstractTableChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AbstractTestFunctionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AbstractTestFunction> objects = new ObjectsCache<AbstractTestFunction>(GetInstances);

        internal static AbstractTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AbstractTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AbstractTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AbstractTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<AbstractTestFunction>GetApparents()
        {
            return
            new AbstractTestFunction[0].Union(
CollectionArgumentedFunctionTest.objects).Union(
EmptyTestFunctionTest.objects).Union(
TestFunctionsSequenceTest.objects).Union(
SimpleTestFunctionTest.objects);
        }

        [TestMethod]
        public void AbstractTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AbstractTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AbstractTestFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddCellExpandableInfoTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddCellExpandableInfo> objects = new ObjectsCache<AddCellExpandableInfo>(GetInstances);

        internal static AddCellExpandableInfo First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddCellExpandableInfo Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddCellExpandableInfo Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AddCellExpandableInfo> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<AddCellExpandableInfo>GetApparents()
        {
            return ReadOnlyList<AddCellExpandableInfo>.Empty;
        }

        [TestMethod]
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
        public void AddCellExpandableInfo_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddCellExpandableInfo_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddCellExpandableInfo_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddExceptionLineTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddExceptionLine> objects = new ObjectsCache<AddExceptionLine>(GetInstances);

        internal static AddExceptionLine First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddExceptionLine Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddExceptionLine Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AddExceptionLine> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


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
            var header = "text 135348";
            var exception = new Exception("Text exception");
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
        public void AddExceptionLine_CheckNullArg_header_135349()
        {
            var header = "text 135348";
            var exception = new Exception("Text exception");
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
        public void AddExceptionLine_CheckNullArg_exception_135350()
        {
            var header = "text 135348";
            var exception = new Exception("Text exception");
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
        public void AddExceptionLine_CheckNullArg_rowReference_135351()
        {
            var header = "text 135348";
            var exception = new Exception("Text exception");
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
            var header = "text 135352";
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
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
        public void AddExceptionLine_CheckNullArg_header_135353()
        {
            var header = "text 135352";
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
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
        public void AddExceptionLine_CheckNullArg_exceptions_135354()
        {
            var header = "text 135352";
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine(header, (IEnumerable<Exception>)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exceptions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exceptions' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddExceptionLine_CheckNullArg_rowReference_135355()
        {
            var header = "text 135352";
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
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
        public void AddExceptionLine_CheckNullArg_exception_135356()
        {
            var exception = new Exception("Text exception");
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
        public void AddExceptionLine_CheckNullArg_rowReference_135357()
        {
            var exception = new Exception("Text exception");
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
        public void AddExceptionLine_CheckNullArg_exceptions_135358()
        {
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
            var rowReference = HtmlRowReferenceTest.First;

            try
            {
                new AddExceptionLine((IEnumerable<Exception>)null, rowReference);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "exceptions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'exceptions' isn't checked for null inputs");
        }

        [TestMethod]
        public void AddExceptionLine_CheckNullArg_rowReference_135359()
        {
            var exceptions = new List<Exception>{ new Exception("Text exception") }.ToReadOnlyList();
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
        public void AddExceptionLine_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddExceptionLine_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddExceptionLine_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddTraceLineTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddTraceLine> objects = new ObjectsCache<AddTraceLine>(GetInstances);

        internal static AddTraceLine First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddTraceLine Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddTraceLine Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AddTraceLine> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


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
            var text = "text 135360";
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
        public void AddTraceLine_CheckNullArg_text()
        {
            var text = "text 135360";
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
        public void AddTraceLine_CheckNullArg_rowReference()
        {
            var text = "text 135360";
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
        public void AddTraceLine_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddTraceLine_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddTraceLine_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AppendRowWithCellsTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AppendRowWithCells> objects = new ObjectsCache<AppendRowWithCells>(GetInstances);

        internal static AppendRowWithCells First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AppendRowWithCells Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AppendRowWithCells Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AppendRowWithCells> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


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
            var cellClass = "text 135361";
            var cellHtmlDatas = new List<String>{ "text 135362" }.ToReadOnlyList();

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
        public void AppendRowWithCells_CheckNullArg_cellClass()
        {
            var cellClass = "text 135361";
            var cellHtmlDatas = new List<String>{ "text 135362" }.ToReadOnlyList();

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
        public void AppendRowWithCells_CheckNullArg_cellHtmlDatas()
        {
            var cellClass = "text 135361";
            var cellHtmlDatas = new List<String>{ "text 135362" }.ToReadOnlyList();

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
        public void AppendRowWithCells_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AppendRowWithCells_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AppendRowWithCells_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CssClassCellChangeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CssClassCellChange> objects = new ObjectsCache<CssClassCellChange>(GetInstances);

        internal static CssClassCellChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CssClassCellChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CssClassCellChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<CssClassCellChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<CssClassCellChange>GetApparents()
        {
            return
            new CssClassCellChange[0].Union(
ShowActualValueCellChangeTest.objects);
        }

        [TestMethod]
        public void CssClassCellChange_CheckNullArg_cell()
        {
            var cell = HtmlCellTest.First;
            var newClass = "text 135363";

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
            var newClass = "text 135363";

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
        public void CssClassCellChange_CheckNullArg_newClass()
        {
            var cell = HtmlCellTest.First;
            var newClass = "text 135363";

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
        public void CssClassCellChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CssClassCellChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CssClassCellChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CollectionArgumentedFunctionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CollectionArgumentedFunction> objects = new ObjectsCache<CollectionArgumentedFunction>(GetInstances);

        internal static CollectionArgumentedFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CollectionArgumentedFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CollectionArgumentedFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<CollectionArgumentedFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<CollectionArgumentedFunction>GetApparents()
        {
            return ReadOnlyList<CollectionArgumentedFunction>.Empty;
        }

        [TestMethod]
        public void CollectionArgumentedFunction_CheckNullArg_columnNames()
        {
            var columnNames = new List<String>{ "text 135364" }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionArgumentedFunction(null, rows, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "columnNames", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'columnNames' isn't checked for null inputs");
        }

        [TestMethod]
        public void CollectionArgumentedFunction_CheckNullArg_rows()
        {
            var columnNames = new List<String>{ "text 135364" }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionArgumentedFunction(columnNames, null, function, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rows", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rows' isn't checked for null inputs");
        }

        [TestMethod]
        public void CollectionArgumentedFunction_CheckNullArg_function()
        {
            var columnNames = new List<String>{ "text 135364" }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionArgumentedFunction(columnNames, rows, null, nullToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "function", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'function' isn't checked for null inputs");
        }

        [TestMethod]
        public void CollectionArgumentedFunction_CheckNullArg_functionToExecute()
        {
            var columnNames = new List<String>{ "text 135364" }.ToReadOnlyList();
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var function = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new CollectionArgumentedFunction(columnNames, rows, function, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionToExecute", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionToExecute' isn't checked for null inputs");
        }

        [TestMethod]
        public void CollectionArgumentedFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CollectionArgumentedFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CollectionArgumentedFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class EmptyTestFunctionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<EmptyTestFunction> objects = new ObjectsCache<EmptyTestFunction>(GetInstances);

        internal static EmptyTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static EmptyTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static EmptyTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<EmptyTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<EmptyTestFunction>GetApparents()
        {
            return ReadOnlyList<EmptyTestFunction>.Empty;
        }

        [TestMethod]
        public void EmptyTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void EmptyTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void EmptyTestFunction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddRowCssClassTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddRowCssClass> objects = new ObjectsCache<AddRowCssClass>(GetInstances);

        internal static AddRowCssClass First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddRowCssClass Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddRowCssClass Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<AddRowCssClass> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<AddRowCssClass>GetApparents()
        {
            return ReadOnlyList<AddRowCssClass>.Empty;
        }

        [TestMethod]
        public void AddRowCssClass_CheckNullArg_rowReference()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var targetCssClass = "text 135365";

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
            var targetCssClass = "text 135365";

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
        public void AddRowCssClass_CheckNullArg_targetCssClass()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var targetCssClass = "text 135365";

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
        public void AddRowCssClass_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddRowCssClass_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddRowCssClass_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ExecutionFailedMessageTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ExecutionFailedMessage> objects = new ObjectsCache<ExecutionFailedMessage>(GetInstances);

        internal static ExecutionFailedMessage First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ExecutionFailedMessage Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ExecutionFailedMessage Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<ExecutionFailedMessage> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<ExecutionFailedMessage>GetApparents()
        {
            return ReadOnlyList<ExecutionFailedMessage>.Empty;
        }

        [TestMethod]
        public void ExecutionFailedMessage_CheckNullArg_rowReference()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135366";
            var messageFormat = "text 135367";
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
            var header = "text 135366";
            var messageFormat = "text 135367";
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
        public void ExecutionFailedMessage_CheckNullArg_header()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135366";
            var messageFormat = "text 135367";
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
            var header = "text 135366";
            var messageFormat = "text 135367";
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
        public void ExecutionFailedMessage_CheckNullArg_messageFormat()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135366";
            var messageFormat = "text 135367";
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
        public void ExecutionFailedMessage_CheckNullArg_args()
        {
            var rowReference = HtmlRowReferenceTest.First;
            var header = "text 135366";
            var messageFormat = "text 135367";
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
        public void ExecutionFailedMessage_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ExecutionFailedMessage_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ExecutionFailedMessage_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ShowActualValueCellChangeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ShowActualValueCellChange> objects = new ObjectsCache<ShowActualValueCellChange>(GetInstances);

        internal static ShowActualValueCellChange First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ShowActualValueCellChange Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ShowActualValueCellChange Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<ShowActualValueCellChange> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<ShowActualValueCellChange>GetApparents()
        {
            return ReadOnlyList<ShowActualValueCellChange>.Empty;
        }

        [TestMethod]
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
        public void ShowActualValueCellChange_CheckNullArg_actualValue()
        {
            var cell = HtmlCellTest.First;
            var actualValue = new object();

            try
            {
                new ShowActualValueCellChange(cell, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "actualValue", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'actualValue' isn't checked for null inputs");
        }

        [TestMethod]
        public void ShowActualValueCellChange_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ShowActualValueCellChange_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ShowActualValueCellChange_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class TestFunctionsSequenceTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<TestFunctionsSequence> objects = new ObjectsCache<TestFunctionsSequence>(GetInstances);

        internal static TestFunctionsSequence First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static TestFunctionsSequence Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static TestFunctionsSequence Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<TestFunctionsSequence> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<TestFunctionsSequence>GetApparents()
        {
            return ReadOnlyList<TestFunctionsSequence>.Empty;
        }

        [TestMethod]
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
        public void TestFunctionsSequence_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void TestFunctionsSequence_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void TestFunctionsSequence_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class SimpleTestFunctionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<SimpleTestFunction> objects = new ObjectsCache<SimpleTestFunction>(GetInstances);

        internal static SimpleTestFunction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static SimpleTestFunction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static SimpleTestFunction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<SimpleTestFunction> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<SimpleTestFunction>GetApparents()
        {
            return ReadOnlyList<SimpleTestFunction>.Empty;
        }

        [TestMethod]
        public void SimpleTestFunction_CheckNullArg_header()
        {
            var header = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new SimpleTestFunction(null, functionToExecute);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "header", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'header' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleTestFunction_CheckNullArg_functionToExecute()
        {
            var header = FunctionHeaderTest.First;
            var functionToExecute = TestFunctionReferenceTest.First;

            try
            {
                new SimpleTestFunction(header, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "functionToExecute", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'functionToExecute' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleTestFunction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void SimpleTestFunction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void SimpleTestFunction_ToStringTest()
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
        internal static readonly ObjectsCache<FunctionExecutionResult> objects = new ObjectsCache<FunctionExecutionResult>(GetInstances);

        internal static FunctionExecutionResult First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static FunctionExecutionResult Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static FunctionExecutionResult Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<FunctionExecutionResult> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<FunctionExecutionResult>GetApparents()
        {
            return ReadOnlyList<FunctionExecutionResult>.Empty;
        }

        [TestMethod]
        public void FunctionExecutionResult_CheckWrongEnumArg_resultType()
        {
            var resultType = EnumHelper<FunctionRunResult>.Values.First();
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();

            try
            {
                new FunctionExecutionResult((NetRunner.Executable.Invokation.FunctionExecutionResult+FunctionRunResult)4, tableChanges);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "resultType", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'resultType' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void FunctionExecutionResult_CheckNullArg_tableChanges()
        {
            var resultType = EnumHelper<FunctionRunResult>.Values.First();
            var tableChanges = new List<AbstractTableChange>{ AbstractTableChangeTest.First }.ToReadOnlyList();

            try
            {
                new FunctionExecutionResult(resultType, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "tableChanges", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'tableChanges' isn't checked for null inputs");
        }

        [TestMethod]
        public void FunctionExecutionResult_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void FunctionExecutionResult_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void FunctionExecutionResult_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class FunctionHeaderTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<FunctionHeader> objects = new ObjectsCache<FunctionHeader>(GetInstances);

        internal static FunctionHeader First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static FunctionHeader Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static FunctionHeader Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<FunctionHeader> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


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
            var functionName = "text 135368";
            var arguments = new List<String>{ "text 135369" }.ToReadOnlyList();
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
        public void FunctionHeader_CheckNullArg_functionName()
        {
            var functionName = "text 135368";
            var arguments = new List<String>{ "text 135369" }.ToReadOnlyList();
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
        public void FunctionHeader_CheckNullArg_arguments()
        {
            var functionName = "text 135368";
            var arguments = new List<String>{ "text 135369" }.ToReadOnlyList();
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
        public void FunctionHeader_CheckNullArg_rowReference()
        {
            var functionName = "text 135368";
            var arguments = new List<String>{ "text 135369" }.ToReadOnlyList();
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
        public void FunctionHeader_CheckNullArg_keyword()
        {
            var functionName = "text 135368";
            var arguments = new List<String>{ "text 135369" }.ToReadOnlyList();
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
        public void FunctionHeader_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void FunctionHeader_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void FunctionHeader_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlRowReferenceTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<HtmlRowReference> objects = new ObjectsCache<HtmlRowReference>(GetInstances);

        internal static HtmlRowReference First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static HtmlRowReference Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static HtmlRowReference Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<HtmlRowReference> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<HtmlRowReference>GetApparents()
        {
            return ReadOnlyList<HtmlRowReference>.Empty;
        }

        [TestMethod]
        public void HtmlRowReference_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void HtmlRowReference_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void HtmlRowReference_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class TestFunctionReferenceTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<TestFunctionReference> objects = new ObjectsCache<TestFunctionReference>(GetInstances);

        internal static TestFunctionReference First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static TestFunctionReference Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static TestFunctionReference Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<TestFunctionReference> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<TestFunctionReference>GetApparents()
        {
            return ReadOnlyList<TestFunctionReference>.Empty;
        }

        [TestMethod]
        public void TestFunctionReference_CheckNullArg_method()
        {
            var method = GetType().Methods.First();
            var targetObject = new FakeFunctionContainer();

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
        public void TestFunctionReference_CheckNullArg_targetObject()
        {
            var method = GetType().Methods.First();
            var targetObject = new FakeFunctionContainer();

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
        public void TestFunctionReference_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void TestFunctionReference_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
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
        internal static readonly ObjectsCache<FitnesseHtmlDocument> objects = new ObjectsCache<FitnesseHtmlDocument>(GetInstances);

        internal static FitnesseHtmlDocument First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static FitnesseHtmlDocument Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static FitnesseHtmlDocument Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<FitnesseHtmlDocument> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<FitnesseHtmlDocument>GetApparents()
        {
            return ReadOnlyList<FitnesseHtmlDocument>.Empty;
        }

        [TestMethod]
        public void FitnesseHtmlDocument_CheckEmptyStringArg_textBeforeFirstTable()
        {
            CheckEmptyStringArg_textBeforeFirstTable(string.Empty);
            CheckEmptyStringArg_textBeforeFirstTable("    ");
            CheckEmptyStringArg_textBeforeFirstTable(Environment.NewLine);
            CheckEmptyStringArg_textBeforeFirstTable("\n\r");
        }

        private void CheckEmptyStringArg_textBeforeFirstTable(string stringArgument)
        {
            var textBeforeFirstTable = "text 135370";
            var tables = new List<HtmlTable>{ HtmlTableTest.First }.ToReadOnlyList();

            try
            {
                new FitnesseHtmlDocument(stringArgument, tables);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "textBeforeFirstTable", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'textBeforeFirstTable' isn't checked for emply values");
        }

        [TestMethod]
        public void FitnesseHtmlDocument_CheckNullArg_textBeforeFirstTable()
        {
            var textBeforeFirstTable = "text 135370";
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
        public void FitnesseHtmlDocument_CheckNullArg_tables()
        {
            var textBeforeFirstTable = "text 135370";
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
        public void FitnesseHtmlDocument_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void FitnesseHtmlDocument_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void FitnesseHtmlDocument_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlCellTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<HtmlCell> objects = new ObjectsCache<HtmlCell>(GetInstances);

        internal static HtmlCell First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static HtmlCell Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static HtmlCell Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<HtmlCell> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<HtmlCell>GetApparents()
        {
            return ReadOnlyList<HtmlCell>.Empty;
        }

        [TestMethod]
        public void HtmlCell_CheckNullArg_tableCell()
        {
            var tableCell = new HtmlNode();

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
        public void HtmlCell_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void HtmlCell_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void HtmlCell_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlRowTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<HtmlRow> objects = new ObjectsCache<HtmlRow>(GetInstances);

        internal static HtmlRow First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static HtmlRow Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static HtmlRow Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<HtmlRow> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<HtmlRow>GetApparents()
        {
            return ReadOnlyList<HtmlRow>.Empty;
        }

        [TestMethod]
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
        public void HtmlRow_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void HtmlRow_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void HtmlRow_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class HtmlTableTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<HtmlTable> objects = new ObjectsCache<HtmlTable>(GetInstances);

        internal static HtmlTable First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static HtmlTable Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static HtmlTable Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<HtmlTable> GetInstances()
        {
            return GetApparents().Concat(GetInstancesOfCurrentType());
        }


        private static IEnumerable<HtmlTable>GetApparents()
        {
            return ReadOnlyList<HtmlTable>.Empty;
        }

        [TestMethod]
        public void HtmlTable_CheckNullArg_rows()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = new HtmlNode();
            var textAfterTable = "text 135371";

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
        public void HtmlTable_CheckNullArg_tableNode()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = new HtmlNode();
            var textAfterTable = "text 135371";

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
        public void HtmlTable_CheckEmptyStringArg_textAfterTable()
        {
            CheckEmptyStringArg_textAfterTable(string.Empty);
            CheckEmptyStringArg_textAfterTable("    ");
            CheckEmptyStringArg_textAfterTable(Environment.NewLine);
            CheckEmptyStringArg_textAfterTable("\n\r");
        }

        private void CheckEmptyStringArg_textAfterTable(string stringArgument)
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = new HtmlNode();
            var textAfterTable = "text 135371";

            try
            {
                new HtmlTable(rows, tableNode, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "textAfterTable", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'textAfterTable' isn't checked for emply values");
        }

        [TestMethod]
        public void HtmlTable_CheckNullArg_textAfterTable()
        {
            var rows = new List<HtmlRow>{ HtmlRowTest.First }.ToReadOnlyList();
            var tableNode = new HtmlNode();
            var textAfterTable = "text 135371";

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
        public void HtmlTable_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void HtmlTable_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void HtmlTable_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}


#pragma warning restore 219
#pragma warning restore 67
// ReSharper restore RedundantCast
// ReSharper restore UnusedVariable
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper restore ObjectCreationAsStatement
// ReSharper restore ConvertToConstant.Local
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global

