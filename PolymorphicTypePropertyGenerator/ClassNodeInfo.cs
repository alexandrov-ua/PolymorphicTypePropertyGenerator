using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PolymorphicTypePropertyGenerator
{
    internal class ClassNodeInfo
    {
        public string DisplayName { get; }
        public string Namespace { get; }
        public string BaseTypeName { get; }

        public ClassNodeInfo(string displayName, string @namespace, string baseTypeName)
        {
            DisplayName = displayName;
            Namespace = @namespace;
            BaseTypeName = baseTypeName;
        }

        public string Name => DisplayName.Split('.').Last();

        public override bool Equals(object obj)
        {
            var val = obj as ClassNodeInfo;
            if (val != null)
            {
                return DisplayName == val.DisplayName && Namespace == val.Namespace && BaseTypeName == val.BaseTypeName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return DisplayName.GetHashCode() ^ Namespace.GetHashCode() ^ BaseTypeName.GetHashCode();
        }
    }
}
