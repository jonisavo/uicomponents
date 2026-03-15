using System.Threading.Tasks;
using UnityEngine;

namespace UIComponents
{
    /// <summary>
    /// Loads assets for UIComponents. Handles both path resolution
    /// and asset loading for a given backend (Resources, AssetDatabase, etc.).
    /// </summary>
    public interface IAssetSource
    {
        Task<T> LoadAsset<T>(string assetPath) where T : Object;
        Task<bool> AssetExists(string assetPath);
    }
}
