using NSubstitute;
using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class LayoutAttributeTests
    {
        [Layout("Assets/MyLayout.uxml")]
        private class UIComponentWithLayout : UIComponent {}
        
        private IAssetResolver _resolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resolver = Substitute.For<IAssetResolver>();
            DependencyInjector.SetDependency<UIComponentWithLayout, IAssetResolver>(_resolver);
        }

        [TearDown]
        public void TearDown()
        {
            _resolver.ClearReceivedCalls();
        }

        [Test]
        public void Given_Layout_Is_Loaded()
        {
            var component = new UIComponentWithLayout();
            _resolver.Received().LoadAsset<VisualTreeAsset>("Assets/MyLayout.uxml");
        }
    }
}