using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentNoAttributesTests
    {
        private partial class UIComponentNoAttributes : UIComponent {}

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
            var testBed = new TestBed<UIComponentNoAttributes>()
                .WithSingleton(_mockResolver);
            _component = testBed.CreateComponent();
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
    }
}
