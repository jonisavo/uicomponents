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

            request.completed += operation => taskCompletionSource.SetResult(request.asset as T);
            
            return taskCompletionSource.Task;
        }

        public Task<bool> AssetExists(string assetPath)
        {
            var request = Resources.LoadAsync(assetPath);
            
            var taskCompletionSource = new TaskCompletionSource<bool>();
            
            request.completed += operation => taskCompletionSource.SetResult(request.asset != null);
            
            return taskCompletionSource.Task;
        }
    }
}
