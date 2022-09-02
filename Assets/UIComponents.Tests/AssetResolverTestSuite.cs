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

            var obj = assetResolver.LoadAsset<TAsset>(assetPath);
            
            Assert.That(obj, Is.Not.Null);
            
            Assert.That(obj, Is.InstanceOf<Task<TAsset>>());

            yield return obj.AsEnumerator();
            
            Assert.That(obj.Result, Is.Not.Null);
            
            Assert.That(obj.Result, Is.InstanceOf<TAsset>());
        }
        
        protected void Assert_Tells_If_Asset_Exists(string assetPath)
        {
            var assetResolver = new T();

            var existingAssetExists =
                assetResolver.AssetExists(assetPath);
            var nonExistantAssetDoesNotExist =
                assetResolver.AssetExists("SomeOtherAsset");
            
            Assert.That(existingAssetExists, Is.True);
            Assert.That(nonExistantAssetDoesNotExist, Is.False);
        }
    }
}
