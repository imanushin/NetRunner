﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    internal interface ITestGenerator
    {
        OutFile[] GetFileEntries(Assembly targetAssembly, ConfigFile configFile);

        string ConfigFileName
        {
            get;
        }
    }
}
