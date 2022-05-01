using UnityEngine;

namespace UIComponents.Core
{
    /// <summary>
    /// An IAssetResolver which loads assets from Resources. Used
    /// by default in UIComponents as a dependency, which can be
    /// overridden.
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="UIComponent"/>
    /// </summary>
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