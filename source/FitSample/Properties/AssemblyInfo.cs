using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using NetRunner.ExternalLibrary;

[assembly: AssemblyTitle("FitSample")]
[assembly: AssemblyDescription("")]

[assembly: Guid("1d3267c3-3f79-4cb8-9c8c-736d0bdbf18d")]

[assembly: ExcludeDefaultFunctionName(false)]
[assembly: ArgumentPrepare(ArgumentPrepareAttribute.ArgumentPrepareMode.CleanupHtmlContent)]