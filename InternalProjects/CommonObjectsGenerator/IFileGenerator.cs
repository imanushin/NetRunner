using System;
using System.Collections.Generic;

namespace CommonObjectsGenerator
{
    public interface IFileGenerator
    {
        string ProjectName
        {
            get;
        }

        string FileName
        {
            get;
        }

        string CreateFileEntry(ISet<Type> types); 
    }
}
