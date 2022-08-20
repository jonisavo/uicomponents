using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class StylesheetAttributeTests
    {
        [Stylesheet("Assets/StylesheetOne.uss")]
        [Stylesheet("Assets/StylesheetTwo.uss")]
        private class UIComponentWithTwoStylesheets : UIComponent {}
        
        [Stylesheet("Assets/StylesheetThree.uss")]
        private class InheritedComponent : UIComponentWithTwoStylesheets {}

        private TestBed _testBed;
        private IAssetResolver _mockResolver;
        private IUIComponentLogger _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<IUIComponentLogger>();
            _mockResolver = MockUtilities.CreateMockResolver();
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetThree.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _testBed = TestBed.Create()
                .WithSingleton(_mockLogger)
                .WithSingleton(_mockResolver)
                .Build();
        }

        [Test]
        public void Given_Stylesheets_Are_Loaded()
        {
            var component = _testBed.CreateComponent<UIComponentWithTwoStylesheets>();
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(2));
        }

        [Test]
        public void Inherited_Stylesheets_Are_Loaded()
        {
            var component = _testBed.CreateComponent<InheritedComponent>();
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetThree.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(3));
        }

        [Test]
        public void Invalid_Stylesheets_Output_Error_Message()
        {
            _mockResolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .ReturnsNull();

            var component = _testBed.CreateComponent<UIComponentWithTwoStylesheets>();

            _mockResolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _mockLogger.Received().LogError("Could not find stylesheet Assets/StylesheetOne.uss", component);

            Assert.That(component.styleSheets.count, Is.EqualTo(1));
        }
    }
}