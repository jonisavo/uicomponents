using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UnityEngine;
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
            _assetResolver = Substitute.For<IAssetResolver>();
            
            _assetResolver.LoadAsset<VisualTreeAsset>("Assets/MyAsset.uxml")
                .Returns(ScriptableObject.CreateInstance<VisualTreeAsset>());
            _assetResolver.LoadAsset<VisualTreeAsset>("Assets/MyOtherAsset.uxml")
                .Returns(ScriptableObject.CreateInstance<VisualTreeAsset>());
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