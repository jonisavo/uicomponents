﻿using System.Collections;
using NUnit.Framework;
using UIComponents.InterfaceModifiers;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor.Interfaces
{
    [TestFixture]
    public partial class UIComponentInterfaceTests
    {
        private sealed class MyCustomEvent : EventBase<MyCustomEvent> {}
    
        [RegistersEventCallback(typeof(MyCustomEvent))]
        private interface IOnMyCustomEvent
        {
            void OnMyCustomEvent(MyCustomEvent evt);
        }
        
        private partial class BaseTestComponent : UIComponent
        {
            public bool Fired { get; protected set; }
        }

        [SetUp]
        public void SetUp()
        {
            _editorWindow = EditorWindow.GetWindow<InterfacesTestEditorWindow>();
        }

        private InterfacesTestEditorWindow _editorWindow;

        [TearDown]
        public void TearDown()
        {
            if (_editorWindow)
                _editorWindow.Close();
            _editorWindow = null;
        }

        private IEnumerator Assert_Registers_Event_Callback<TComponent, TEvent>()
            where TComponent : BaseTestComponent, new()
            where TEvent : EventBase<TEvent>, new()
        {
            var component = new TComponent();
            Assert.That(component.Fired, Is.False);
            _editorWindow.AddTestComponent(component);

            yield return component.WaitForInitializationEnumerator();

            using (var evt = new TEvent())
            {
                evt.target = component;
                component.SendEvent(evt);
            }

            Assert.That(component.Fired, Is.True);
        }

        private partial class UIComponentWithOnGeometryChanged : BaseTestComponent, IOnGeometryChanged
        {
            public void OnGeometryChanged(GeometryChangedEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnGeometryChanged_Registers_GeometryChangedEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnGeometryChanged, GeometryChangedEvent>();
        }

        private partial class UIComponentWithOnAttachToPanel : BaseTestComponent, IOnAttachToPanel
        {
            public void OnAttachToPanel(AttachToPanelEvent evt) => Fired = true;
        }
        
        [UnityTest]
        public IEnumerator IOnAttachToPanel_Registers_AttachToPanelEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnAttachToPanel, AttachToPanelEvent>();
        }

        private partial class UIComponentWithOnDetachFromPanel : BaseTestComponent, IOnDetachFromPanel
        {
            public void OnDetachFromPanel(DetachFromPanelEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnDetachFromPanel_Registers_DetachFromPanelEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnDetachFromPanel, DetachFromPanelEvent>();
        }
        
        private partial class UIComponentWithOnMouseEnter : BaseTestComponent, IOnMouseEnter
        {
            public void OnMouseEnter(MouseEnterEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnMouseEnter_Registers_MouseEnterEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnMouseEnter, MouseEnterEvent>();
        }
        
        private partial class UIComponentWithOnMouseLeave : BaseTestComponent, IOnMouseLeave
        {
            public void OnMouseLeave(MouseLeaveEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnMouseLeave_Registers_MouseLeaveEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnMouseLeave, MouseLeaveEvent>();
        }
        
        private partial class UIComponentWithOnClick : BaseTestComponent, IOnClick
        {
            public void OnClick(ClickEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnClick_Registers_ClickEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnClick, ClickEvent>();
        }

        private partial class UIComponentWithOnMyCustomEvent : BaseTestComponent, IOnMyCustomEvent
        {
            public void OnMyCustomEvent(MyCustomEvent evt) => Fired = true;
        }
        
        [UnityTest]
        public IEnumerator IOnMyCustomEvent_Registers_MyCustomEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnMyCustomEvent, MyCustomEvent>();
        }

#if UNITY_2021_3_OR_NEWER
        private partial class UIComponentWithOnNavigationMove : BaseTestComponent, IOnNavigationMove
        {
            public void OnNavigationMove(NavigationMoveEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnNavigationMove_Registers_NavigationMoveEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnNavigationMove, NavigationMoveEvent>();
        }
#endif
    }
}
