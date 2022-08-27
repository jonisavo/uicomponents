using System.Threading.Tasks;

namespace UIComponents
{
    /// <summary>
    /// An interface for classes whose purpose is to load assets
    /// and determine whether they exist.
    /// </summary>
    public interface IAssetResolver
    {
        /// <summary>
        /// Loads the asset.
        /// </summary>
        /// <param name="assetPath">Asset path</param>
        /// <typeparam name="T">Asset type</typeparam>
        /// <returns>A task which resolves to the asset object</returns>
        Task<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object;

        /// <summary>
        /// Returns whether the asset exists.
        /// </summary>
        /// <param name="assetPath">Asset path</param>
        /// <returns>Whether the asset exists</returns>
        bool AssetExists(string assetPath);
    }
}
