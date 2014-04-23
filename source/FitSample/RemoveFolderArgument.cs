using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class RemoveFolderArgument : BaseTableArgument
    {
        private readonly string baseDirectoryPath;

        public RemoveFolderArgument(string baseDirectoryPath)
        {
            this.baseDirectoryPath = baseDirectoryPath;
        }

        public bool RemoveFolder(string folderName)
        {
            var targetFolder = Path.Combine(baseDirectoryPath, folderName);

            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder);

                return true;
            }

            return false;
        }
    }
}
