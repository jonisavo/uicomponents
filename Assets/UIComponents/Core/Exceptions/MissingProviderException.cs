using System;

namespace UIComponents.Core.Exceptions
{
    /// <summary>
    /// Thrown when a dependency does not have a provider.
    /// </summary>
    public class MissingProviderException : Exception
    {
        /// <summary>
        /// The dependency type.
        /// </summary>
        public readonly Type DependencyType;
        
        public MissingProviderException(Type dependencyType)
        {
            DependencyType = dependencyType;
            Message = $"No provider found for {dependencyType.Name}";
        }

        public override string Message { get; }
    }
}