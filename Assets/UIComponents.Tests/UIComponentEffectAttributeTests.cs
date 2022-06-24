using System;
using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentEffectAttributeTests
    {
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        private class TestEffectAttribute : UIComponentEffectAttribute
        {
            public override void Apply(UIComponent component) {}
        }
        
        [TestEffect]
        [TestEffect(Priority = -2)]
        [TestEffect(Priority = 5)]
        private class UIComponentWithEffects : UIComponent {}

        [Test]
        public void Effects_Are_Sorted_By_Priority()
        {
            var component = new UIComponentWithEffects();
            
            Assert.That(UIComponent.TryGetCache<UIComponentWithEffects>(out var cache), Is.True);
            
            Assert.That(cache.EffectAttributes.Count, Is.EqualTo(3));
            Assert.That(cache.EffectAttributes[0].Priority, Is.EqualTo(5));
            Assert.That(cache.EffectAttributes[1].Priority, Is.EqualTo(0));
            Assert.That(cache.EffectAttributes[2].Priority, Is.EqualTo(-2));
        }
    }
}