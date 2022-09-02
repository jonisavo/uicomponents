namespace UIComponents
{
    /// <summary>
    /// An enum for dependency scopes.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// A singleton dependency, which is cached. All consumers receive
        /// the same instance.
        /// </summary>
        Singleton,
        /// <summary>
        /// A dependency which is not cached. All consumers receive their
        /// own instance.
        /// </summary>
        Transient
    }
}