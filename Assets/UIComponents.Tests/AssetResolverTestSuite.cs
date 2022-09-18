using System.Collections;
using System.Threading.Tasks;
using NUnit.Framework;
using UIComponents.Internal;

namespace UIComponents.Tests
{
    public abstract class AssetResolverTestSuite<T> where T : IAssetResolver, new()
    {
        protected IEnumerator Assert_Loads_Existing_Asset<TAsset>(string assetPath)
            where TAsset : UnityEngine.Object
        {
            var assetResolver = new T();

            var loadTask = assetResolver.LoadAsset<TAsset>(assetPath);
            
            Assert.That(loadTask, Is.Not.Null);

            yield return loadTask.AsEnumerator();
            
            Assert.That(loadTask.Result, Is.Not.Null);
            
            Assert.That(loadTask.Result, Is.InstanceOf<TAsset>());
        }
        
        protected IEnumerator Assert_Tells_If_Asset_Exists(string assetPath)
        {
            var assetResolver = new T();

            var existingAssetExistsTask =
                assetResolver.AssetExists(assetPath);
            var nonExistantAssetDoesNotExistTask =
                assetResolver.AssetExists("SomeOtherAsset");
            
            Assert.That(existingAssetExistsTask, Is.Not.Null);
            Assert.That(nonExistantAssetDoesNotExistTask, Is.Not.Null);

            yield return Task.WhenAll(existingAssetExistsTask, nonExistantAssetDoesNotExistTask).AsEnumerator();
            
            Assert.That(existingAssetExistsTask.Result, Is.True);
            Assert.That(nonExistantAssetDoesNotExistTask.Result, Is.False);
        }
    }
}
