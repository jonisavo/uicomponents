using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UIComponents.Tests.Utilities;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class LayoutAttributeTests
    {
        [Layout("Assets/MyAsset.uxml")]
        private class UIComponentWithLayout : UIComponent {}
        
        private class InheritedComponentWithoutAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MyOtherAsset.uxml")]
        private class InheritedComponentWithAttribute : UIComponentWithLayout {}
        
        [Layout("Assets/MissingAsset.uxml")]
        private class UIComponentWithNullLayout : UIComponent {}

        private IAssetResolver _assetResolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _assetResolver = MockUtilities.CreateMockResolver();
            
            _assetResolver.LoadAsset<VisualTreeAsset>("Assets/MissingAsset.uxml")
                .ReturnsNull();
            
            DependencyInjector.SetDependency<UIComponentWithLayout, IAssetResolver>(
                _assetResolver
            );
            DependencyInjector.SetDependency<InheritedComponentWithoutAttribute, IAssetResolver>(
                _assetResolver
            );
            DependencyInjector.SetDependency<InheritedComponentWithAttribute, IAssetResolver>(
                _assetResolver
            );
            DependencyInjector.SetDependency<UIComponentWithNullLayout, IAssetResolver>(
                _assetResolver
            );
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DependencyInjector.RestoreDefaultDependency<UIComponentWithLayout, IAssetResolver>();
            DependencyInjector.RestoreDefaultDependency<InheritedComponentWithoutAttribute, IAssetResolver>();
            DependencyInjector.RestoreDefaultDependency<InheritedComponentWithAttribute, IAssetResolver>();
            DependencyInjector.RestoreDefaultDependency<UIComponentWithNullLayout, IAssetResolver>();
        }
        
        [TearDown]
        public void TearDown()
        {
            _assetResolver.ClearReceivedCalls();
        }

        [Test]
        public void Given_Layout_Is_Loaded()
        {
            var component = new UIComponentWithLayout();
            _assetResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [Test]
        public void Superclass_Layout_Is_Loaded_If_It_Is_Not_Overridden()
        {
            var component = new InheritedComponentWithoutAttribute();
            _assetResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [Test]
        public void Superclass_Layout_Is_Not_Loaded_If_Overridden()
        {
            var component = new InheritedComponentWithAttribute();
            _assetResolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyOtherAsset.uxml");
            _assetResolver.DidNotReceive().LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml");
        }

        [Test]
        public void Null_Layout_Is_Handled()
        {
            Assert.DoesNotThrow(() => new UIComponentWithNullLayout());
        }
    }
}