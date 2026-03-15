using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentNoAttributesTests
    {
        private partial class UIComponentNoAttributes : UIComponent {}

        private IAssetSource _mockSource;
        private UIComponentNoAttributes _component;

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            _mockSource = Substitute.For<IAssetSource>();
            _mockSource.LoadAsset<VisualTreeAsset>(Arg.Any<string>())
                .Returns(Task.FromResult<VisualTreeAsset>(null));
            _mockSource.LoadAsset<StyleSheet>(Arg.Any<string>())
                .Returns(Task.FromResult<StyleSheet>(null));
            var testBed = new TestBed<UIComponentNoAttributes>()
                .WithSingleton(_mockSource);
            _component = testBed.Instantiate();
            yield return _component.Initialize().AsEnumerator();
        }

        [TearDown]
        public void TearDown()
        {
            _mockSource.ClearReceivedCalls();
        }

        [Test]
        public void No_Layout_Is_Loaded()
        {
            Assert.That(_component.childCount, Is.Zero);
            _mockSource.DidNotReceive().LoadAsset<VisualTreeAsset>(Arg.Any<string>());
        }

        [Test]
        public void No_Styles_Are_Loaded()
        {
            Assert.That(_component.styleSheets.count, Is.Zero);
            _mockSource.DidNotReceive().LoadAsset<StyleSheet>(Arg.Any<string>());
        }
    }
}
