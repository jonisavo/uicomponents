using System;
using JetBrains.Annotations;

namespace UIComponents.Core
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    public class AssetPathAttribute : Attribute
    {
        public readonly string Path;
        
        public AssetPathAttribute(string path)
        {
            Path = path;
        }
    }
}