using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UIComponents.Internal;
using UnityEngine.TestTools;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentEffectAttributeTests
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        private class TestEffectAttribute : UIComponentEffectAttribute
        {
            public override void Apply(UIComponent component)
            {
                if (component is UIComponentWithEffects componentWithEffects)
                    componentWithEffects.AppliedEffects.Add(Priority);
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        private class AnotherTestEffectAttribute : TestEffectAttribute
        {
            public override int Priority { get; set; } = 999;
        }

        [TestEffect]
        [TestEffect(Priority = -2)]
        [AnotherTestEffect]
        [TestEffect(Priority = 5)]
        private partial class UIComponentWithEffects : UIComponent
        {
            public readonly List<int> AppliedEffects = new List<int>();
        }

        [UnityTest]
        public IEnumerator Effects_Are_Sorted_By_Priority()
        {
            var component = new UIComponentWithEffects();
            yield return component.Initialize().AsEnumerator();

            Assert.That(component.AppliedEffects.Count, Is.EqualTo(4));
            Assert.That(component.AppliedEffects[0], Is.EqualTo(999));
            Assert.That(component.AppliedEffects[1], Is.EqualTo(5));
            Assert.That(component.AppliedEffects[2], Is.EqualTo(0));
            Assert.That(component.AppliedEffects[3], Is.EqualTo(-2));
        }
    }
}
