using System.Threading.Tasks;
using UnityEngine;

namespace UIComponents
{
    /// <summary>
    /// An IAssetSource which loads assets from Resources.
    /// Default asset source for UIComponents.
    /// </summary>
    /// <seealso cref="DependencyAttribute"/>
    /// <seealso cref="UIComponent"/>
    public class ResourcesAssetSource : IAssetSource
    {
        public Task<T> LoadAsset<T>(string assetPath) where T : Object
        {
            var request = Resources.LoadAsync<T>(assetPath);
            var taskCompletionSource = new TaskCompletionSource<T>();
            request.completed += _ => taskCompletionSource.SetResult(request.asset as T);
            return taskCompletionSource.Task;
        }

        public Task<bool> AssetExists(string assetPath)
        {
            var request = Resources.LoadAsync(assetPath);
            var taskCompletionSource = new TaskCompletionSource<bool>();
            request.completed += _ => taskCompletionSource.SetResult(request.asset != null);
            return taskCompletionSource.Task;
        }
    }
}
