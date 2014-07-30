using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{

    [Serializable]
    public sealed class TypeData : IHelpIdentity
    {
        private const string typeIdentityFormat = "T:{0}";
        
        internal TypeData(Type type)
        {
            Name = type.Name;
            FullName = type.FullName;

            HelpIdentity = string.Format(typeIdentityFormat, type.FullName);
            Properties = type.GetProperties().Select(PropertyReference.GetPropertyReference).ToList().AsReadOnly();
        }

        public string Name
        {
            get;
            private set;
        }

        public string FullName
        {
            get;
            private set;
        }

        public string HelpIdentity
        {
            get;
            private set;
        }

        public ReadOnlyCollection<PropertyReference> Properties
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

    }
}
