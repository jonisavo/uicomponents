using NSubstitute;
using NUnit.Framework;
using UIComponents.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class LayoutAttributeTests
    {
        [Layout("Assets/MyAsset.uxml")]
        private class UIComponentWithLayout : UIComponent {}

        private IAssetResolver _assetResolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _assetResolver = Substitute.For<IAssetResolver>();
            DependencyInjector.SetDependency<UIComponentWithLayout, IAssetResolver>(
                _assetResolver
            );
        }

        [Test]
        public void Given_Layout_Is_Loaded()
        {
            _assetResolver.LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml")
                .Returns(ScriptableObject.CreateInstance<VisualTreeAsset>());
            
            var component = new UIComponentWithLayout();

            _assetResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }
    }
}