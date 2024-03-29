﻿using System.Collections;
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
    }
}
