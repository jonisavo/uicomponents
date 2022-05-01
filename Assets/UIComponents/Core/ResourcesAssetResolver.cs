using UnityEngine;

namespace UIComponents.Core
{
    public class ResourcesAssetResolver : IAssetResolver
    {
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            return Resources.Load<T>(assetPath);
        }

        public bool AssetExists(string assetPath)
        {
            return Resources.Load(assetPath) != null;
        }
    }
}