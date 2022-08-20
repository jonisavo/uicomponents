using System;
using JetBrains.Annotations;

namespace UIComponents.DependencyInjection
{
    /// <summary>
    /// An internal class for provided dependencies.
    /// </summary>
    internal class Dependency
    {
        /// <summary>
        /// The instance of the dependency which is provided to consumers.
        /// </summary>
        public object Instance { get; private set; }
        
        /// <summary>
        /// The type of the provider the dependency was initialized
        /// with.
        /// </summary>
        public Type InitialProviderType { get; }
        
        /// <summary>
        /// The scope of the dependency.
        /// </summary>
        public Scope Scope { get; }

        private readonly Type _dependencyType;
        
        /// <summary>
        /// Initializes a new Dependency with a provided instance.
        /// </summary>
        /// <param name="dependencyType">Dependency type</param>
        /// <param name="instance">Provided instance</param>
        /// <param name="scope">Dependency scope</param>
        /// <exception cref="ArgumentNullException">Thrown if arguments are null</exception>
        /// <exception cref="ArgumentException">Thrown if the instance is not of the dependency type</exception>
        public Dependency([NotNull] Type dependencyType, [NotNull] object instance, Scope scope)
        {
            if (dependencyType == null)
                throw new ArgumentNullException(nameof(dependencyType));
            
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            
            _dependencyType = dependencyType;
            Scope = scope;
            
            if (!_dependencyType.IsInstanceOfType(instance))
                throw new ArgumentException("Instance is not of type " + _dependencyType.Name);
            
            Instance = instance;
            InitialProviderType = instance.GetType();
        }

        /// <summary>
        /// Initializes a new Dependency with a provider type.
        /// Activator.CreateInstance is used to create an instance.
        /// </summary>
        /// <param name="dependencyType">Dependency type</param>
        /// <param name="providerType">Provider type</param>
        /// <param name="scope">Dependency scope</param>
        /// <exception cref="ArgumentNullException">Thrown if arguments are null</exception>
        public Dependency([NotNull] Type dependencyType, [NotNull] Type providerType, Scope scope) :
            this(dependencyType, Activator.CreateInstance(providerType), scope) {}

        /// <summary>
        /// Changes the provided instance. It must be of the correct type.
        /// </summary>
        /// <param name="instance">New instance to provide</param>
        /// <exception cref="ArgumentNullException">Thrown if instance is null</exception>
        /// <exception cref="ArgumentException">Thrown if instance is not of the dependency type</exception>
        public void ChangeInstance([NotNull] object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            
            if (!_dependencyType.IsInstanceOfType(instance))
                throw new ArgumentException("Instance is not of type " + _dependencyType.Name);

            Instance = instance;
        }

        /// <summary>
        /// Clears the provided instance.
        /// </summary>
        public void Clear()
        {
            Instance = null;
        }
    }
}