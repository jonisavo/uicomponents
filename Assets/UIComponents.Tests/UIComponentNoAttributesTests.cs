using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentNoAttributesTests
    {
        private class UIComponentNoAttributes : UIComponent {}

        private TestBed _testBed;
        private IAssetResolver _mockResolver;
        private UIComponentNoAttributes _component;

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            _mockResolver = Substitute.For<IAssetResolver>();
            _mockResolver.LoadAsset<VisualTreeAsset>(Arg.Any<string>())
                .Returns(Task.FromResult<VisualTreeAsset>(null));
            _mockResolver.LoadAsset<StyleSheet>(Arg.Any<string>())
                .Returns(Task.FromResult<StyleSheet>(null));
            _testBed = TestBed.Create()
                .WithSingleton(_mockResolver)
                .Build();
            _component = _testBed.CreateComponent<UIComponentNoAttributes>();
            yield return _component.WaitForInitializationEnumerator();
        }

        [TearDown]
        public void TearDown()
        {
            _mockResolver.ClearReceivedCalls();
        }

        [Test]
        public void No_Layout_Is_Loaded()
        {
            Assert.That(_component.childCount, Is.Zero);
            _mockResolver.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Styles_Are_Loaded()
        {
            Assert.That(_component.styleSheets.count, Is.Zero);
            _mockResolver.DidNotReceive().LoadAsset<StyleSheet>(Arg.Any<string>());
        }

        [Test]
        public void No_Asset_Paths_Exist()
        {
            Assert.That(_component.GetAssetPaths().Count(), Is.EqualTo(0));
        }
    }
}
