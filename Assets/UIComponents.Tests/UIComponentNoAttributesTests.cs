using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentNoAttributesTests
    {
        private class UIComponentNoAttributes : UIComponent {}

        private TestBed _testBed;
        private IAssetResolver _mockResolver;

        [SetUp]
        public void SetUp()
        {
            _mockResolver = Substitute.For<IAssetResolver>();
            _testBed = TestBed.Create()
                .WithSingleton(_mockResolver)
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            _mockResolver.ClearReceivedCalls();
        }

        [Test]
        public void No_Layout_Is_Loaded()
        {
            var component = _testBed.CreateComponent<UIComponentNoAttributes>();
            Assert.That(component.childCount, Is.Zero);
            _mockResolver.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Styles_Are_Loaded()
        {
            var component = _testBed.CreateComponent<UIComponentNoAttributes>();
            Assert.That(component.styleSheets.count, Is.Zero);
            _mockResolver.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Asset_Paths_Exist()
        {
            var component = _testBed.CreateComponent<UIComponentNoAttributes>();
            Assert.That(component.GetAssetPaths().Count(), Is.EqualTo(0));
        }
    }
}