using System.Collections;
using NUnit.Framework;
using UIComponents.Addressables;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using UIComponents.Internal;

namespace UIComponents.Tests.Addressables
{
    [TestFixture]
    public class AddressableAssetResolverTests : AssetResolverTestSuite<AddressableAssetResolver>
    {
        [UnityTest]
        public IEnumerator Should_Be_Able_To_Load_Existing_Asset()
        {
            yield return Assert_Loads_Existing_Asset<StyleSheet>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            yield return Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }

        [UnityTest]
        public IEnumerator Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            yield return Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            yield return Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }

        [UnityTest]
        public IEnumerator Caches_Asset_Existence_Checks()
        {
            var resolver = new AddressableAssetResolver();

            var existsPath = "Assets/UIComponents.Tests/Addressables/Assets/Component.uss";
            var existsTask = resolver.AssetExists(existsPath);

            yield return existsTask.AsEnumerator();
            
            Assert.That(resolver.AssetPathExistsCache[existsPath], Is.True);
            
            var notExistsPath = "Foobar";
            var notExistsTask = resolver.AssetExists(notExistsPath);
            
            yield return notExistsTask.AsEnumerator();
            
            Assert.That(resolver.AssetPathExistsCache[notExistsPath], Is.False);

            var existsTaskTwo = resolver.AssetExists(existsPath);

            Assert.That(existsTaskTwo.Result, Is.True);
        }
    }
}
