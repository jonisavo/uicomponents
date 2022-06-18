using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UIComponents.Tests.Utilities;
using UIComponents.Utilities;
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
        
        private IAssetResolver _resolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resolver = MockUtilities.CreateMockResolver();
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetThree.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            DependencyInjector.SetDependency<UIComponentWithTwoStylesheets, IAssetResolver>(_resolver);
            DependencyInjector.SetDependency<InheritedComponent, IAssetResolver>(_resolver);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DependencyInjector.RestoreDefaultDependency<UIComponentWithTwoStylesheets, IAssetResolver>();
            DependencyInjector.RestoreDefaultDependency<InheritedComponent, IAssetResolver>();
        }
        
        [TearDown]
        public void TearDown()
        {
            _resolver.ClearReceivedCalls();
        }

        [Test]
        public void Given_Stylesheets_Are_Loaded()
        {
            var component = new UIComponentWithTwoStylesheets();
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(2));
        }

        [Test]
        public void Inherited_Stylesheets_Are_Loaded()
        {
            var component = new InheritedComponent();
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss");
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetThree.uss");
            Assert.That(component.styleSheets.count, Is.EqualTo(3));
        }

        [Test]
        public void Invalid_Stylesheets_Output_Error_Message()
        {
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .ReturnsNull();

            var mockLogger = Substitute.For<IUIComponentLogger>();
            
            UIComponentWithTwoStylesheets component;

            using (new DependencyScope<UIComponentWithTwoStylesheets, IUIComponentLogger>(mockLogger))
                component = new UIComponentWithTwoStylesheets();

            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            mockLogger.Received().LogError("Could not find stylesheet Assets/StylesheetOne.uss", component);

            Assert.That(component.styleSheets.count, Is.EqualTo(1));
        }
    }
}