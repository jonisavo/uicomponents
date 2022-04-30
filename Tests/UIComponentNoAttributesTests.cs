using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentNoAttributesTests
    {
        private class UIComponentNoAttributes : UIComponent {}
        
        private IAssetResolver _resolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resolver = Substitute.For<IAssetResolver>();
            DependencyInjector.SetDependency<UIComponentNoAttributes, IAssetResolver>(_resolver);
        }
        
        [TearDown]
        public void TearDown()
        {
            _resolver.ClearReceivedCalls();
        }

        [Test]
        public void No_Layout_Is_Loaded()
        {
            var component = new UIComponentNoAttributes();
            Assert.That(component.childCount, Is.Zero);
            _resolver.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Styles_Are_Loaded()
        {
            var component = new UIComponentNoAttributes();
            Assert.That(component.styleSheets.count, Is.Zero);
            _resolver.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Asset_Paths_Exist()
        {
            var component = new UIComponentNoAttributes();
            Assert.That(component.GetAssetPaths().Count(), Is.EqualTo(0));
        }
    }
}