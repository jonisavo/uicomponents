using System.Collections;
using System.Linq;
using NUnit.Framework;
using UIComponents.Editor;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor
{
    [Dependency(typeof(IAssetSource), provide: typeof(AssetDatabaseAssetSource))]
    [AssetRoot("Assets/UIComponents.Tests/Editor/ConventionTestAssets/")]
    [Layout]
    [Stylesheet]
    internal partial class ConventionTestComponent : UIComponent {}

    [Dependency(typeof(IAssetSource), provide: typeof(AssetDatabaseAssetSource))]
    [AssetRoot("Assets/UIComponents.Tests/Editor/ConventionTestAssets/")]
    [Layout]
    [Stylesheet]
    [SharedStylesheet("SharedTestStyle")]
    internal partial class FlatComponent : UIComponent {}

    [Dependency(typeof(IAssetSource), provide: typeof(AssetDatabaseAssetSource))]
    [AssetRoot("Assets/UIComponents.Tests/Editor/ConventionTestAssets/")]
    [Layout]
    [Stylesheet]
    internal partial class MissingConventionComponent : UIComponent {}

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

        [Test]
        public void ConventionValidator_Resolves_Nested_Convention_Paths()
        {
            var results = ConventionValidator.ValidateAll()
                .Where(result => result.ComponentType == typeof(ConventionTestComponent))
                .ToArray();

            Assert.That(results, Has.Length.EqualTo(2));
            Assert.That(results.All(result => result.Exists), Is.True);
            Assert.That(results.All(result => !result.IsAmbiguous), Is.True);

            var layoutResult = results.Single(result => result.AssetKind == "Layout");
            Assert.That(layoutResult.ResolvedPath,
                Is.EqualTo("Assets/UIComponents.Tests/Editor/ConventionTestAssets/ConventionTestComponent/ConventionTestComponent.uxml"));

            var stylesheetResult = results.Single(result => result.AssetKind == "Stylesheet");
            Assert.That(stylesheetResult.ResolvedPath,
                Is.EqualTo("Assets/UIComponents.Tests/Editor/ConventionTestAssets/ConventionTestComponent/ConventionTestComponent.style.uss"));
        }

        [Test]
        public void ConventionValidator_Resolves_Flat_Convention_And_Shared_Stylesheet_Paths()
        {
            var results = ConventionValidator.ValidateAll()
                .Where(result => result.ComponentType == typeof(FlatComponent))
                .ToArray();

            Assert.That(results, Has.Length.EqualTo(3));
            Assert.That(results.All(result => result.Exists), Is.True);
            Assert.That(results.All(result => !result.IsAmbiguous), Is.True);

            var resolvedPaths = results
                .Select(result => result.ResolvedPath)
                .OrderBy(path => path)
                .ToArray();

            Assert.That(resolvedPaths, Is.EqualTo(new[]
            {
                "Assets/UIComponents.Tests/Editor/ConventionTestAssets/FlatComponent.style.uss",
                "Assets/UIComponents.Tests/Editor/ConventionTestAssets/FlatComponent.uxml",
                "Assets/UIComponents.Tests/Editor/ConventionTestAssets/SharedTestStyle.uss"
            }));
        }

        [Test]
        public void ConventionValidator_Reports_Missing_Convention_Paths()
        {
            var results = ConventionValidator.ValidateAll()
                .Where(result => result.ComponentType == typeof(MissingConventionComponent))
                .ToArray();

            Assert.That(results, Has.Length.EqualTo(2));
            Assert.That(results.All(result => !result.Exists), Is.True);
            Assert.That(results.All(result => !result.IsAmbiguous), Is.True);
            Assert.That(results.All(result => result.ResolvedPath == null), Is.True);
        }
    }
}
