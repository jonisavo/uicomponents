using System.Collections.Generic;
using UIComponents.Core;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UIComponents.Addressables
{
    public class AddressableAssetResolver : IAssetResolver
    {
        private readonly List<AsyncOperationHandle> _handles
            = new List<AsyncOperationHandle>();

        ~AddressableAssetResolver()
        {
            foreach (var handle in _handles)
                UnityEngine.AddressableAssets.Addressables.Release(handle);
        }
        
        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(assetPath);
            
            _handles.Add(handle);

            return handle.WaitForCompletion();
        }
        
        public bool AssetExists(string assetPath)
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(assetPath);

            var locations = handle.WaitForCompletion();
            
            UnityEngine.AddressableAssets.Addressables.Release(handle);

            return locations.Count > 0;
        }
    }
}

