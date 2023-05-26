using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UIComponents.Editor
{
    /// <summary>
    /// An IAssetResolver which loads assets with AssetDatabase.
    /// </summary>
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="UIComponent"/>
    public class AssetDatabaseAssetResolver : IAssetResolver
    {
        public Task<T> LoadAsset<T>(string assetPath) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            return Task.FromResult(asset);
        }

        public Task<bool> AssetExists(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            return Task.FromResult(asset != null);
        }
    }
}
