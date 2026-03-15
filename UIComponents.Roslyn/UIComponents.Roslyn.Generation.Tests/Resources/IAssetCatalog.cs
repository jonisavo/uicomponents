namespace UIComponents
{
    public interface IAssetCatalog
    {
        string ResolveLayoutPath(Type componentType, string path);
        string ResolveStylesheetPath(Type componentType, string path);
        string ResolveSharedStylesheetPath(string name, string path);
    }

    public class DefaultAssetCatalog : IAssetCatalog
    {
        public string ResolveLayoutPath(Type componentType, string path) => path;
        public string ResolveStylesheetPath(Type componentType, string path) => path;
        public string ResolveSharedStylesheetPath(string name, string path) => path;
    }
}
