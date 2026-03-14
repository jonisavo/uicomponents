using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class StylesheetAttributeTests
    {
        [Stylesheet("Assets/StylesheetOne.uss")]
        [Stylesheet("Assets/StylesheetTwo.uss")]
        private partial class UIComponentWithTwoStylesheets : UIComponent {}
        
        [Stylesheet("Assets/StylesheetThree.uss")]
        private partial class InheritedComponent : UIComponentWithTwoStylesheets {}

        [Stylesheet]
        private partial class UIComponentWithConventionStylesheet : UIComponent {}

        [Stylesheet("Assets/StylesheetOne.uss")]
        [SharedStylesheet("Assets/SharedStyle.uss")]
        private partial class UIComponentWithSharedStylesheet : UIComponent {}

        [SharedStylesheet("Assets/BaseShared.uss")]
        private partial class BaseWithSharedStylesheet : UIComponent {}

        [Stylesheet("Assets/ChildStyle.uss")]
        [SharedStylesheet("Assets/ChildShared.uss")]
        private partial class ChildWithSharedStylesheet : BaseWithSharedStylesheet {}

        private IAssetResolver _mockResolver;
        private ILogger _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger>();
            _mockResolver = MockUtilities.CreateMockResolver();
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetThree.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
        }

        [UnityTest]
        public IEnumerator Given_Stylesheets_Are_Loaded()
        {
            var testBed = new TestBed<UIComponentWithTwoStylesheets>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Inherited_Stylesheets_Are_Loaded()
        {
            var testBed = new TestBed<InheritedComponent>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetThree.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Invalid_Stylesheets_Output_Error_Message()
        {
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .Returns(Task.FromResult<StyleSheet>(null));

            var testBed = new TestBed<UIComponentWithTwoStylesheets>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);

            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockLogger.Received().LogError("Could not find stylesheet Assets/StylesheetOne.uss", component);

            Assert.That(component.styleSheets.count, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator Convention_Stylesheet_Uses_Class_Name_With_Style_Suffix()
        {
            _mockResolver.LoadAsset<StyleSheet>("UIComponentWithConventionStylesheet.style")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));

            var testBed = new TestBed<UIComponentWithConventionStylesheet>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("UIComponentWithConventionStylesheet.style");
            Assert.That(component.styleSheets.count, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator SharedStylesheet_Is_Loaded()
        {
            _mockResolver.LoadAsset<StyleSheet>("Assets/SharedStyle.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));

            var testBed = new TestBed<UIComponentWithSharedStylesheet>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/SharedStyle.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Inherited_SharedStylesheets_Are_Loaded()
        {
            _mockResolver.LoadAsset<StyleSheet>("Assets/BaseShared.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
            _mockResolver.LoadAsset<StyleSheet>("Assets/ChildStyle.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));
            _mockResolver.LoadAsset<StyleSheet>("Assets/ChildShared.uss")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<StyleSheet>()));

            var testBed = new TestBed<ChildWithSharedStylesheet>()
                .WithSingleton(_mockLogger)
                .WithTransient(_mockResolver);
            var component = testBed.Instantiate();
            yield return component.Initialize().AsEnumerator();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/BaseShared.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/ChildStyle.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/ChildShared.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(3));
        }
    }
}
