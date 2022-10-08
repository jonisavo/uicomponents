﻿using System.Diagnostics.CodeAnalysis;

namespace UIComponents.Experimental
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class UxmlNameAttribute : Attribute
    {
        public readonly string Name;

        public UxmlNameAttribute(string name)
        {
            Name = name;
        }
    }
}
