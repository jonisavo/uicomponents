using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UIComponents
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
        public Task<T> LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            var request = Resources.LoadAsync<T>(assetPath);

            var taskCompletionSource = new TaskCompletionSource<T>();

            request.completed += delegate(AsyncOperation operation)
            {
                if (operation.isDone) {
                    taskCompletionSource.SetResult(request.asset as T);
                } else {
                    taskCompletionSource.SetException(new Exception("Failed to load asset"));
                }
            };
            
            return taskCompletionSource.Task;
        }

        public bool AssetExists(string assetPath)
        {
            return Resources.Load(assetPath) != null;
        }
    }
}
