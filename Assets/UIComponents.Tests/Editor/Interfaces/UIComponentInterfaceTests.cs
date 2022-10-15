using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;
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

            using (var evt = new TEvent() { target = component })
                component.SendEvent(evt);

            Assert.That(component.Fired, Is.True);
        }
        
        private class UIComponentWithOnGeometryChanged : BaseTestComponent, IOnGeometryChanged
        {
            public void OnGeometryChanged(GeometryChangedEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnGeometryChanged_Registers_GeometryChangedEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnGeometryChanged, GeometryChangedEvent>();
        }

        private class UIComponentWithOnAttachToPanel : BaseTestComponent, IOnAttachToPanel
        {
            public void OnAttachToPanel(AttachToPanelEvent evt) => Fired = true;
        }
        
        [UnityTest]
        public IEnumerator IOnAttachToPanel_Registers_AttachToPanelEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnAttachToPanel, AttachToPanelEvent>();
        }

        private class UIComponentWithOnDetachFromPanel : BaseTestComponent, IOnDetachFromPanel
        {
            public void OnDetachFromPanel(DetachFromPanelEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnDetachFromPanel_Registers_DetachFromPanelEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnDetachFromPanel, DetachFromPanelEvent>();
        }
        
        private class UIComponentWithOnMouseEnter : BaseTestComponent, IOnMouseEnter
        {
            public void OnMouseEnter(MouseEnterEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnMouseEnter_Registers_MouseEnterEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnMouseEnter, MouseEnterEvent>();
        }
        
        private class UIComponentWithOnMouseLeave : BaseTestComponent, IOnMouseLeave
        {
            public void OnMouseLeave(MouseLeaveEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnMouseLeave_Registers_MouseLeaveEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnMouseLeave, MouseLeaveEvent>();
        }
        
        private class UIComponentWithOnClick : BaseTestComponent, IOnClick
        {
            public void OnClick(ClickEvent evt) => Fired = true;
        }

        [UnityTest]
        public IEnumerator IOnClick_Registers_ClickEvent_Callback()
        {
            yield return Assert_Registers_Event_Callback<UIComponentWithOnClick, ClickEvent>();
        }
        
#if UNITY_2021_3_OR_NEWER
        private class UIComponentWithOnNavigationMove : BaseTestComponent, IOnNavigationMove
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
