using System.Collections;
using NUnit.Framework;
using UIComponents.Editor;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor
{
    [TestFixture]
    public class AssetDatabaseAssetSourceTests : AssetSourceTestSuite<AssetDatabaseAssetSource>
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

        [Test]
        public void Resolves_Convention_Layout_In_Nested_Directory()
        {
            var source = new AssetDatabaseAssetSource();
            var task = source.LoadAsset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Editor/ConventionTestAssets/ConventionTestComponent");
            Assert.That(task.Result, Is.Not.Null);
        }

        [Test]
        public void Resolves_Convention_Layout_In_Flat_Directory()
        {
            var source = new AssetDatabaseAssetSource();
            var task = source.LoadAsset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Editor/ConventionTestAssets/FlatComponent");
            Assert.That(task.Result, Is.Not.Null);
        }

        [Test]
        public void Passes_Through_Explicit_Path()
        {
            var source = new AssetDatabaseAssetSource();
            var task = source.LoadAsset<StyleSheet>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss");
            Assert.That(task.Result, Is.Not.Null);
        }

        [Test]
        public void Returns_Null_For_Unresolvable_Convention_Path()
        {
            var source = new AssetDatabaseAssetSource();
            var task = source.LoadAsset<VisualTreeAsset>("Assets/NonExistent/SomeComponent");
            Assert.That(task.Result, Is.Null);
        }
    }
}
