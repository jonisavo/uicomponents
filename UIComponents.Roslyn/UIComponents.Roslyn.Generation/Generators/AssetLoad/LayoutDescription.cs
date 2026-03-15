namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    internal readonly struct LayoutDescription
    {
        public readonly string Path;
        public readonly string DeclaringTypeFullName;

        public LayoutDescription(string path, string declaringTypeFullName)
        {
            Path = path;
            DeclaringTypeFullName = declaringTypeFullName;
        }
    }
}
