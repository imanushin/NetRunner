using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class CreateFolderArgument : BaseTableArgument
    {
        private readonly string baseDirectoryPath;

        public CreateFolderArgument(string baseDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(baseDirectoryPath))
            {
                throw new ArgumentException("Argument is empty", "baseDirectoryPath");
            }

            this.baseDirectoryPath = baseDirectoryPath;

            Directory.Exists(baseDirectoryPath);
        }
        
        public void CreateFolder(string folderName)
        {
            Directory.CreateDirectory(Path.Combine(baseDirectoryPath, folderName));
        }

    }
}
