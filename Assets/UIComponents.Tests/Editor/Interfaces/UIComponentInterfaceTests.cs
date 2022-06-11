using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor.Interfaces
{
    [TestFixture]
    public class UIComponentInterfaceTests
    {
        private class BaseTestComponent : UIComponent
        {
            public bool Fired { get; protected set; }
        }

        private static void Assert_Registers_Event_Callback<TComponent, TEvent>()
            where TComponent : BaseTestComponent, new()
            where TEvent : EventBase<TEvent>, new()
        {
            var window = EditorWindow.GetWindow<InterfacesTestEditorWindow>();
            var component = new TComponent();
            Assert.That(component.Fired, Is.False);
            window.AddTestComponent(component);

            using (var evt = new TEvent() { target = component })
                component.SendEvent(evt);

            Assert.That(component.Fired, Is.True);
            window.Close();
        }
        
        private class UIComponentWithOnGeometryChanged : BaseTestComponent, IOnGeometryChanged
        {
            public void OnGeometryChanged(GeometryChangedEvent evt) => Fired = true;
        }

        [Test]
        public void IOnGeometryChanged_Registers_GeometryChangedEvent_Callback()
        {
            Assert_Registers_Event_Callback<UIComponentWithOnGeometryChanged, GeometryChangedEvent>();
        }

        private class UIComponentWithOnAttachToPanel : BaseTestComponent, IOnAttachToPanel
        {
            public void OnAttachToPanel(AttachToPanelEvent evt) => Fired = true;
        }
        
        [Test]
        public void IOnAttachToPanel_Registers_AttachToPanelEvent_Callback()
        {
            Assert_Registers_Event_Callback<UIComponentWithOnAttachToPanel, AttachToPanelEvent>();
        }

        private class UIComponentWithOnDetachFromPanel : BaseTestComponent, IOnDetachFromPanel
        {
            public void OnDetachFromPanel(DetachFromPanelEvent evt) => Fired = true;
        }

        [Test]
        public void IOnDetachFromPanel_Registers_DetachFromPanelEvent_Callback()
        {
            Assert_Registers_Event_Callback<UIComponentWithOnDetachFromPanel, DetachFromPanelEvent>();
        }
    }
}