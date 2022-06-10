using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class QueryClassAttributeTests
    {
        [Layout("UIComponentTests/QueryClassAttributeTest")]
        private class QueryClassTestComponent : UIComponent
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

            [Query(Class = "class1")]
            [Query(Class = "class2")]
            [Query(Class = "class3")]
            public VisualElement[] AllElements;

            [Query(Class = "class1")]
            [Query(Class = "class2")]
            [Query(Class = "class3")]
            public Label[] AllLabels;

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

        [Test]
        public void Populates_Array_With_All_Elements_With_Class()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.AllClassOneElementsArray, Is.Not.Null);
            Assert.That(component.AllClassOneElementsArray.Length, Is.EqualTo(4));
        }

        [Test]
        public void Populates_List_With_All_Elements_With_Class()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.AllClassOneElementsList, Is.Not.Null);
            Assert.That(component.AllClassOneElementsList.Count, Is.EqualTo(4));
        }
        
        [Test]
        public void Populates_Single_Element_With_First_Element_With_Class()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.ClassOneElement, Is.Not.Null);
            Assert.That(component.ClassOneElement.name, Is.EqualTo("class1-first"));
        }

        [Test]
        public void Populates_Typed_Array_And_List_With_Elements_Of_Type_And_Class()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.ClassOneLabelArray, Is.Not.Null);
            Assert.That(component.ClassOneLabelArray.Length, Is.EqualTo(3));
            AssertAllElementsAreOfType(component.ClassOneLabelArray);
            
            Assert.That(component.ClassOneLabelList, Is.Not.Null);
            Assert.That(component.ClassOneLabelList.Count, Is.EqualTo(3));
            AssertAllElementsAreOfType(component.ClassOneLabelList);
        }
        
        
        [Test]
        public void Populates_Array_With_All_Elements_With_Multiple_Classes()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.AllElements, Is.Not.Null);
            Assert.That(component.AllElements.Length, Is.EqualTo(7));
        }
        
        [Test]
        public void Populates_Typed_Array_With_Multiple_Classes()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.AllLabels, Is.Not.Null);
            Assert.That(component.AllLabels.Length, Is.EqualTo(5));
            AssertAllElementsAreOfType(component.AllLabels);
        }

        [Test]
        public void Creates_Empty_List_And_Array_If_No_Elements_Are_Found()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.EmptyLabelArray, Is.Not.Null);
            Assert.That(component.EmptyLabelArray.Length, Is.EqualTo(0));
            Assert.That(component.EmptyLabelList, Is.Not.Null);
            Assert.That(component.EmptyLabelList.Count, Is.EqualTo(0));
        }

        [Test]
        public void Leaves_Field_Null_If_No_Element_Is_Found()
        {
            var component = new QueryClassTestComponent();
            
            Assert.That(component.EmptyLabel, Is.Null);
        }

        [Layout("UIComponentTests/QueryClassAttributeTest")]
        private class UIComponentWithNameAndClassQuery : UIComponent
        {
            [Query("class1-first")]
            [Query(Class = "class2")]
            [Query(Class = "class3")]
            public readonly Label[] Labels;
        }
        
        [Test]
        public void Works_With_Both_Name_And_Class_Query()
        {
            var component = new UIComponentWithNameAndClassQuery();
            
            Assert.That(component.Labels, Is.Not.Null);
            Assert.That(component.Labels.Length, Is.EqualTo(3));
        }
    }
}