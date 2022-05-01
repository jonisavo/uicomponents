using System;

namespace UIComponents.Core.Exceptions
{
    public class MissingProviderException : Exception
    {
        public readonly Type DependencyType;
        
        public MissingProviderException(Type dependencyType)
        {
            DependencyType = dependencyType;
            Message = $"No provider found for {dependencyType.Name}";
        }

        public override string Message { get; }
    }
}