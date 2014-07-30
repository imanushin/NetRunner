using System.Reflection;
using NetRunner.ExternalLibrary;
using NetRunner.ExternalLibrary.Properties;

namespace NetRunner.TestExecutionProxy
{
    public sealed class PropertyData : IHelpIdentity
    {
        private const string propertyFormat = "P:{0}.{1}";

        public PropertyData(PropertyInfo property)
        {
            Name = property.Name;
            Owner = TypeReference.GetType(property.DeclaringType);
            ArgumentPrepareMode = ReflectionHelpers.FindAttribute(property, ArgumentPrepareAttribute.Default).Mode;
            TrimInputCharacters = ReflectionHelpers.FindAttribute(property, StringTrimAttribute.Default).TrimInputString;
            HelpIdentity = string.Format(propertyFormat, Owner.TargetType.FullName, property.Name);
            HasGet = property.GetMethod != null;
            PropertyType = TypeReference.GetType(property.PropertyType);
            
        }

        public string HelpIdentity
        {
            get;
            private set;
        }


        public TypeReference Owner
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool HasGet
        {
            get;
            private set;
        }

        [NotNull]
        public TypeReference PropertyType
        {
            get;
            private set;
        }

        public ArgumentPrepareAttribute.ArgumentPrepareMode ArgumentPrepareMode
        {
            get;
            private set;
        }

        public bool TrimInputCharacters
        {
            get;
            private set;
        }

    }
}