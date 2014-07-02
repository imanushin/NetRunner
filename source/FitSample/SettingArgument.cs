using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class SettingArgument : BaseTableArgument
    {
        private readonly Dictionary<string, string> settings;

        public SettingArgument(Dictionary<string, string> settings)
        {
            this.settings = settings;
        }
        
        public void SetSettings(string name, string inValue)
        {
            settings[name.ToLowerInvariant()] = inValue;
        }

        public void GetSettings(string name, out string outValue)
        {
            outValue = settings[name.ToLowerInvariant()];
        }
    }
}
