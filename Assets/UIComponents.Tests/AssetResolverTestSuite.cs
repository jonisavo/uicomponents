using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    public abstract class AssetResolverTestSuite<T> where T : IAssetResolver, new()
    {
        protected void Assert_Should_Be_Able_To_Load_Existing_Asset(string assetPath)
        {
            var assetResolver = new T();

            var layout = assetResolver.LoadAsset<VisualTreeAsset>(assetPath);
            
            Assert.That(layout, Is.Not.Null);

            var visualElement = new VisualElement();
            layout.CloneTree(visualElement);

            Assert.That(visualElement.childCount, Is.EqualTo(1));

            var label = visualElement.Q<Label>();

            Assert.That(label, Is.Not.Null);
            Assert.That(label.text, Is.EqualTo("Hello world!"));
        }
        
        protected void Assert_Should_Be_Able_To_Tell_If_Asset_Exists(string assetPath)
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