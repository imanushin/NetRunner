using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSample
{
    internal sealed class CreateFolderArgument
    {
        private readonly string baseDirectoryPath;

        public CreateFolderArgument(string baseDirectoryPath)
        {
            this.baseDirectoryPath = baseDirectoryPath;
        }

        public void CreateFolder(string folderName)
        {
            Directory.CreateDirectory(Path.Combine(baseDirectoryPath, folderName));
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
