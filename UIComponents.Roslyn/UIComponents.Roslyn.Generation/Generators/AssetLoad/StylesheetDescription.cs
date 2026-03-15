namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    internal readonly struct StylesheetDescription
    {
        public readonly string Path;
        public readonly string DeclaringTypeFullName;
        public readonly bool IsShared;
        public readonly string LogicalName;

        public StylesheetDescription(string path, string declaringTypeFullName)
        {
            Path = path;
            DeclaringTypeFullName = declaringTypeFullName;
            IsShared = false;
            LogicalName = null;
        }

        public StylesheetDescription(string path, string logicalName, bool isShared)
        {
            Path = path;
            DeclaringTypeFullName = null;
            IsShared = isShared;
            LogicalName = logicalName;
        }
    }
}
