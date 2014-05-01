using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    internal sealed class OutFile
    {
        public OutFile(string fileLocalPath, string fileEntry, bool overrideExisting)
        {
            FileLocalPath = fileLocalPath;
            FileEntry = fileEntry;
            OverrideExisting = overrideExisting;
        }

        public string FileLocalPath
        {
            get;
            private set;
        }

        public string FileEntry
        {
            get;
            private set;
        }

        public bool OverrideExisting
        {
            get;
            private set;
        }
    }
}
