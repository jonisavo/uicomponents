using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class QueryClassAttributeTests
    {
        [Layout("UIComponentTests/QueryClassAttributeTest")]
        private partial class QueryClassTestComponent : UIComponent
        {
            [Query(Class = "class1")]
            public VisualElement[] AllClassOneElementsArray;

            [Query(Class = "class1")]
            public List<VisualElement> AllClassOneElementsList;

            [Query(Class = "class1")]
            public VisualElement ClassOneElement;

            [Query(Class = "class1")]
            public Label[] ClassOneLabelArray;

            [Query(Class = "class1")]
            public List<Label> ClassOneLabelList;

            [Query(Class = "no-such-class")]
            public Label[] EmptyLabelArray;

            [Query(Class = "no-such-class")]
            public List<Label> EmptyLabelList;
            
            [Query(Class = "no-such-class")]
            public Label EmptyLabel;
        }

        private void AssertAllElementsAreOfType<T>(IEnumerable<T> elements) where T : VisualElement
        {
            foreach (var element in elements)
                Assert.That(element, Is.InstanceOf<T>());
        }

        private TestBed _testBed;
        private QueryClassTestComponent _queryClassTestComponent;
        
        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            _testBed = TestBed.Create().Build();
            _queryClassTestComponent = _testBed.CreateComponent<QueryClassTestComponent>();
            yield return _queryClassTestComponent.WaitForInitializationEnumerator();
        }

        [Test]
        public void Populates_Array_With_All_Elements_With_Class()
        {
            Assert.That(_queryClassTestComponent.AllClassOneElementsArray, Is.Not.Null);
            Assert.That(_queryClassTestComponent.AllClassOneElementsArray.Length, Is.EqualTo(4));
        }

        [Test]
        public void Populates_List_With_All_Elements_With_Class()
        {
            Assert.That(_queryClassTestComponent.AllClassOneElementsList, Is.Not.Null);
            Assert.That(_queryClassTestComponent.AllClassOneElementsList.Count, Is.EqualTo(4));
        }
        
        [Test]
        public void Populates_Single_Element_With_First_Element_With_Class()
        {
            Assert.That(_queryClassTestComponent.ClassOneElement, Is.Not.Null);
            Assert.That(_queryClassTestComponent.ClassOneElement.name, Is.EqualTo("class1-first"));
        }

        [Test]
        public void Populates_Typed_Array_And_List_With_Elements_Of_Type_And_Class()
        {
            Assert.That(_queryClassTestComponent.ClassOneLabelArray, Is.Not.Null);
            Assert.That(_queryClassTestComponent.ClassOneLabelArray.Length, Is.EqualTo(3));
            AssertAllElementsAreOfType(_queryClassTestComponent.ClassOneLabelArray);
            
            Assert.That(_queryClassTestComponent.ClassOneLabelList, Is.Not.Null);
            Assert.That(_queryClassTestComponent.ClassOneLabelList.Count, Is.EqualTo(3));
            AssertAllElementsAreOfType(_queryClassTestComponent.ClassOneLabelList);
        }

        [Test]
        public void Creates_Empty_List_And_Array_If_No_Elements_Are_Found()
        {
            Assert.That(_queryClassTestComponent.EmptyLabelArray, Is.Not.Null);
            Assert.That(_queryClassTestComponent.EmptyLabelArray.Length, Is.EqualTo(0));
            Assert.That(_queryClassTestComponent.EmptyLabelList, Is.Not.Null);
            Assert.That(_queryClassTestComponent.EmptyLabelList.Count, Is.EqualTo(0));
        }

        [Test]
        public void Leaves_Field_Null_If_No_Element_Is_Found()
        {
            Assert.That(_queryClassTestComponent.EmptyLabel, Is.Null);
        }

        [Layout("UIComponentTests/QueryClassAttributeTest")]
        private partial class UIComponentWithNameAndClassQuery : UIComponent
        {
            [Query("class3-second", Class = "class3")]
            public Label[] Labels;
        }
        
        [UnityTest]
        public IEnumerator Works_With_Both_Name_And_Class_Query()
        {
            var component = _testBed.CreateComponent<UIComponentWithNameAndClassQuery>();
            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.Labels, Is.Not.Null);
            Assert.That(component.Labels.Length, Is.EqualTo(1));
        }
    }
}
