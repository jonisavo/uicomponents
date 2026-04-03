using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UIComponents.Addressables;
using UIComponents.Editor;
using UnityEditor;
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

    [Dependency(typeof(IAssetSource), provide: typeof(AssetDatabaseAssetSource))]
    [AssetRoot("Assets/UIComponents.Tests/Editor/AmbiguousConventionTestAssets/")]
    [Layout]
    internal partial class AmbiguousConventionComponent : UIComponent {}

    [Dependency(typeof(IAssetSource), provide: typeof(AddressableAssetSource))]
    [Layout("Assets/UIComponents.Tests/Addressables/Assets/Component.uxml")]
    [Stylesheet("Assets/UIComponents.Tests/Addressables/Assets/Component.uss")]
    internal partial class AddressableValidatedComponent : UIComponent {}

    [Dependency(typeof(IAssetSource), provide: typeof(AddressableAssetSource))]
    [Layout("Assets/Samples/Resources/Resources/Components/ResourcesExampleComponent.uxml")]
    internal partial class NonAddressableProjectAssetComponent : UIComponent {}

    [TestFixture]
    public class AssetDatabaseAssetSourceTests : AssetSourceTestSuite<AssetDatabaseAssetSource>
    {
        private const string AmbiguousConventionRoot =
            "Assets/UIComponents.Tests/Editor/AmbiguousConventionTestAssets";

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
        public void Resolves_Convention_Path_By_Requested_Asset_Type()
        {
            var source = new AssetDatabaseAssetSource();

            var layoutTask = source.LoadAsset<VisualTreeAsset>(
                "Assets/Samples/Addressables/Data/AddressablesExampleComponent");
            Assert.That(layoutTask.Result, Is.Not.Null);

            var stylesheetTask = source.LoadAsset<StyleSheet>(
                "Assets/Samples/Addressables/Data/AddressablesExampleComponent");
            Assert.That(stylesheetTask.Result, Is.Not.Null);
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
        public void Returns_Null_And_False_For_Ambiguous_Convention_Path()
        {
            CreateAmbiguousConventionAssets();

            try
            {
                var source = new AssetDatabaseAssetSource();
                const string path =
                    "Assets/UIComponents.Tests/Editor/AmbiguousConventionTestAssets/AmbiguousConventionComponent";

                var loadTask = source.LoadAsset<VisualTreeAsset>(path);
                var existsTask = source.AssetExists(path);

                Assert.That(loadTask.Result, Is.Null);
                Assert.That(existsTask.Result, Is.False);
            }
            finally
            {
                DeleteAmbiguousConventionAssets();
            }
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

        [Test]
        public void ConventionValidator_Reports_Ambiguous_Convention_Paths()
        {
            CreateAmbiguousConventionAssets();

            try
            {
                var results = ConventionValidator.ValidateAll()
                    .Where(result => result.ComponentType == typeof(AmbiguousConventionComponent))
                    .ToArray();

                Assert.That(results, Has.Length.EqualTo(1));
                Assert.That(results[0].Exists, Is.False);
                Assert.That(results[0].IsAmbiguous, Is.True);
                Assert.That(results[0].ResolvedPath, Is.Null);
                Assert.That(results[0].CandidatePaths, Is.EqualTo(new[]
                {
                    "Assets/UIComponents.Tests/Editor/AmbiguousConventionTestAssets/OptionA/AmbiguousConventionComponent.uxml",
                    "Assets/UIComponents.Tests/Editor/AmbiguousConventionTestAssets/OptionB/AmbiguousConventionComponent.uxml"
                }));
            }
            finally
            {
                DeleteAmbiguousConventionAssets();
            }
        }

        [Test]
        public void ConventionValidator_Resolves_Addressable_Keys()
        {
            var results = ConventionValidator.ValidateAll()
                .Where(result => result.ComponentType == typeof(AddressableValidatedComponent))
                .ToArray();

            Assert.That(results, Has.Length.EqualTo(2));
            Assert.That(results.All(result => result.Exists), Is.True);
            Assert.That(results.All(result => !result.IsAmbiguous), Is.True);

            var resolvedPaths = results
                .Select(result => result.ResolvedPath)
                .OrderBy(path => path)
                .ToArray();

            Assert.That(resolvedPaths, Is.EqualTo(new[]
            {
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss",
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            }));
        }

        [Test]
        public void ConventionValidator_Does_Not_Treat_NonAddressable_Project_Assets_As_Valid()
        {
            var results = ConventionValidator.ValidateAll()
                .Where(result => result.ComponentType == typeof(NonAddressableProjectAssetComponent))
                .ToArray();

            Assert.That(results, Has.Length.EqualTo(1));
            Assert.That(results[0].Exists, Is.False);
            Assert.That(results[0].IsAmbiguous, Is.False);
            Assert.That(results[0].ResolvedPath, Is.Null);
        }

        [Test]
        public void ValidateAndReport_Summarizes_Current_Validation_Findings()
        {
            CreateAmbiguousConventionAssets();

            try
            {
                var results = ConventionValidator.ValidateAll();
                var summary = ConventionValidator.ValidateAndReport();

                Assert.That(summary.TotalCount, Is.EqualTo(results.Count));
                Assert.That(summary.UnresolvedCount,
                    Is.EqualTo(results.Count(result => !result.Exists && !result.IsAmbiguous)));
                Assert.That(summary.AmbiguousCount,
                    Is.EqualTo(results.Count(result => result.IsAmbiguous)));
                Assert.That(summary.HasFailures, Is.True);
            }
            finally
            {
                DeleteAmbiguousConventionAssets();
            }
        }

        private static void CreateAmbiguousConventionAssets()
        {
            DeleteAmbiguousConventionAssets();

            AssetDatabase.CreateFolder("Assets/UIComponents.Tests/Editor", "AmbiguousConventionTestAssets");
            AssetDatabase.CreateFolder(AmbiguousConventionRoot, "OptionA");
            AssetDatabase.CreateFolder(AmbiguousConventionRoot, "OptionB");

            File.WriteAllText(
                AmbiguousConventionRoot + "/OptionA/AmbiguousConventionComponent.uxml",
                MinimalUxml);
            File.WriteAllText(
                AmbiguousConventionRoot + "/OptionB/AmbiguousConventionComponent.uxml",
                MinimalUxml);

            AssetDatabase.Refresh();
        }

        private static void DeleteAmbiguousConventionAssets()
        {
            if (AssetDatabase.IsValidFolder(AmbiguousConventionRoot))
            {
                AssetDatabase.DeleteAsset(AmbiguousConventionRoot);
                AssetDatabase.Refresh();
            }
        }

        private const string MinimalUxml =
            "<ui:UXML xmlns:ui=\"UnityEngine.UIElements\"><ui:VisualElement /></ui:UXML>";
    }
}
