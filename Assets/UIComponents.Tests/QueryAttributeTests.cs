using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class QueryAttributeTests
    {
        [Layout("UIComponentTests/LayoutAttributeTests")]
        private partial class ComponentWithQueryAttribute : UIComponent
        {
            [Query("hello-world-label")]
            public Label HelloWorldLabel;
            
            [Query(Name = "test-foldout")]
            public Foldout TestFoldout;

            [Query]
            public Label FirstLabel;

            [Query(Class = "text")]
            public Label[] LabelsWithTextClass;

            [Query(Name = "hello-world-label", Class = "text")]
            public Label HelloWorldLabelWithTextClass;

            [Query]
            public List<Label> AllLabelsImplicit;

            [Query(Name = "hello-world-label")]
            [Query(Name = "foldout-content")]
            public List<Label> AllLabelsExplicit;
        }

        private ILogger _mockLogger;
        
        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger>();
        }

        [UnityTest]
        public IEnumerator Should_Populate_Fields()
        {
            var testBed = new TestBed<ComponentWithQueryAttribute>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            component.Initialize();

            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.HelloWorldLabel, Is.InstanceOf<Label>());
            Assert.That(component.HelloWorldLabel.text, Is.EqualTo("Hello world!"));

            Assert.That(component.TestFoldout, Is.InstanceOf<Foldout>());
            
            Assert.That(component.FirstLabel, Is.SameAs(component.HelloWorldLabel));
            
            Assert.That(component.LabelsWithTextClass, Is.Not.Null);
            Assert.That(component.LabelsWithTextClass.Length, Is.EqualTo(2));
            
            Assert.That(component.HelloWorldLabelWithTextClass, Is.SameAs(component.HelloWorldLabel));

            Assert.That(component.AllLabelsImplicit, Is.InstanceOf<List<Label>>());
            Assert.That(component.AllLabelsImplicit.Count, Is.EqualTo(2));
            Assert.That(component.AllLabelsImplicit[1].text, Is.EqualTo("Foldout content"));

            Assert.That(component.AllLabelsExplicit, Is.InstanceOf<List<Label>>());
            Assert.That(component.AllLabelsExplicit.Count, Is.EqualTo(2));
            Assert.That(component.AllLabelsExplicit[1].text, Is.EqualTo("Foldout content"));
        }

        private partial class ChildComponentWithQueryAttribute : ComponentWithQueryAttribute
        {
            [Query("foldout-content")]
            public Label FoldoutContent;
        }
        
        [UnityTest]
        public IEnumerator Should_Populate_Inherited_Fields()
        {
            var testBed = new TestBed<ChildComponentWithQueryAttribute>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            component.Initialize();

            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.HelloWorldLabel, Is.InstanceOf<Label>());
            Assert.That(component.TestFoldout, Is.InstanceOf<Foldout>());
            Assert.That(component.FoldoutContent, Is.InstanceOf<Label>());
            Assert.That(component.FoldoutContent.text, Is.EqualTo("Foldout content"));
        }

        [Layout("UIComponentTests/LayoutAttributeTests")]
        private partial class ComponentWithInvalidQueryAttribute : UIComponent
        {
            [Query]
            public object InvalidField;

            [Query]
            public object[] InvalidArray;

            [Query]
            public List<List<VisualElement>> InvalidList;
        }

        [UnityTest]
        public IEnumerator Should_Not_Populate_Invalid_Fields()
        {
            var testBed = new TestBed<ComponentWithInvalidQueryAttribute>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            component.Initialize();
            
            yield return component.WaitForInitializationEnumerator();

            Assert.That(component.InvalidField, Is.Null);
            Assert.That(component.InvalidArray, Is.Null);
            Assert.That(component.InvalidList, Is.Null);
        }

        private partial class ComponentWithMissingFields : UIComponent
        {
            [Query]
            public Label label;

            [Query]
            public ComponentWithQueryAttribute[] components;

            [Query]
            public List<Button> buttons;
        }

        [UnityTest]
        public IEnumerator Should_Log_Errors_If_Query_Yields_No_Results()
        {
            var testBed = new TestBed<ComponentWithMissingFields>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            component.Initialize();

            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.label, Is.Null);
            Assert.That(component.components.Length, Is.EqualTo(0));
            Assert.That(component.buttons.Count, Is.EqualTo(0));
            
            _mockLogger.Received().LogError("Query (label): No instances of Label found", component);
            _mockLogger.Received().LogError("Query (components): No instances of UIComponents.Tests.QueryAttributeTests.ComponentWithQueryAttribute found", component);
            _mockLogger.Received().LogError("Query (buttons): No instances of Button found", component);
        }
    }
}
