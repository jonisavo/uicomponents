using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UIComponents.Addressables
{
    /// <summary>
    /// An IAssetResolver which loads assets with Addressables.
    /// </summary>
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="UIComponent"/>
    public class AddressableAssetResolver : IAssetResolver
    {
        /// <summary>
        /// A cache of asset existence checks.
        /// </summary>
        public readonly Dictionary<string, bool> AssetPathExistsCache =
            new Dictionary<string, bool>();
        
        public async Task<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(assetPath);

            T asset;

            try
            {
                asset = await handle.Task;
            }
            finally
            {
                UnityEngine.AddressableAssets.Addressables.Release(handle);
            }

            return asset;
        }

        public async Task<bool> AssetExists(string assetPath)
        {
            if (AssetPathExistsCache.TryGetValue(assetPath, out var assetExists))
                return assetExists;
            
            var handle = UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(assetPath);

            IList<IResourceLocation> locations;

            try
            {
                locations = await handle.Task;
            }
            finally
            {
                UnityEngine.AddressableAssets.Addressables.Release(handle); 
            }
            
            var exists = locations.Count > 0;
            
#if UNITY_2021_3_OR_NEWER
            AssetPathExistsCache.TryAdd(assetPath, exists);
#else
            if (!AssetPathExistsCache.ContainsKey(assetPath))
                AssetPathExistsCache.Add(assetPath, exists);
#endif

            return exists;
        }
    }
}

