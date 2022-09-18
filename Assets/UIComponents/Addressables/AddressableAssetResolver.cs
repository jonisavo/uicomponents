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

            return locations.Count > 0;
        }
    }
}

