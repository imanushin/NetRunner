using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary;

namespace FitSample
{
    internal sealed class SettingsContainer : BaseTestContainer
    {
        private static readonly Dictionary<string,string> settings = new Dictionary<string, string>();

        public SettingArgument CheckSettings()
        {
            return new SettingArgument(settings);
        }
    }
}
