using System.Text.RegularExpressions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class StylesheetAttributeTests
    {
        [Stylesheet("Assets/StylesheetOne.uss")]
        [Stylesheet("Assets/StylesheetTwo.uss")]
        private class UIComponentWithTwoStylesheets : UIComponent {}
        
        private IAssetResolver _resolver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resolver = Substitute.For<IAssetResolver>();
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetTwo.uss")
                .Returns(ScriptableObject.CreateInstance<StyleSheet>());
            DependencyInjector.SetDependency<UIComponentWithTwoStylesheets, IAssetResolver>(_resolver);
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
        public void Invalid_Stylesheets_Output_Error_Message()
        {
            _resolver.LoadAsset<StyleSheet>("Assets/StylesheetOne.uss")
                .ReturnsNull();
            
            var component = new UIComponentWithTwoStylesheets();
            
            _resolver.Received().LoadAsset<StyleSheet>("Assets/StylesheetOne.uss");
            
            LogAssert.Expect(LogType.Error, new Regex("Could not find stylesheet"));
            
            Assert.That(component.styleSheets.count, Is.EqualTo(1));
        }
    }
}