namespace UIComponents.Core
{
    public interface IAssetLoader
    {
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object;
    }
}