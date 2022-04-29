namespace UIComponents.Core
{
    public interface IAssetResolver
    {
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object;

        public bool AssetExists(string assetPath);
    }
}