using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class ResourcesAssetResolverTests
    {
        [Test]
        public void Should_Be_Able_To_Load_Existing_Asset()
        {
            var assetResolver = new ResourcesAssetResolver();

            var layout = assetResolver.LoadAsset<VisualTreeAsset>(
                "UIComponentTests/LayoutAttributeTests"
            );

            var visualElement = new VisualElement();
            layout.CloneTree(visualElement);

            Assert.That(visualElement.childCount, Is.EqualTo(1));

            var label = visualElement.Q<Label>();

            Assert.That(label, Is.Not.Null);
            Assert.That(label.text, Is.EqualTo("Hello world!"));
        }

        [Test]
        public void Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            var assetResolver = new ResourcesAssetResolver();

            var existingAssetExists =
                assetResolver.AssetExists("UIComponentTests/LayoutAttributeTests");
            var nonExistantAssetDoesNotExist =
                assetResolver.AssetExists("SomeOtherAsset");
            
            Assert.That(existingAssetExists, Is.True);
            Assert.That(nonExistantAssetDoesNotExist, Is.False);
        }
    }
}