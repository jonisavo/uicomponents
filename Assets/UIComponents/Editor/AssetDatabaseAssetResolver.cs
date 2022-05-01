using UIComponents.Core;
using UnityEditor;

namespace UIComponents.Editor
{
    /// <summary>
    /// An IAssetResolver which loads assets from AssetDatabase.
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="UIComponent"/>
    /// </summary>
    public class AssetDatabaseAssetResolver : IAssetResolver
    {
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public bool AssetExists(string assetPath)
        {
            return !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetPath));
        }
    }
}