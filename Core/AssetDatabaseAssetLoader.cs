using UnityEditor;

namespace UIComponents.Core
{
    public class AssetDatabaseAssetLoader : IAssetLoader
    {
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
}