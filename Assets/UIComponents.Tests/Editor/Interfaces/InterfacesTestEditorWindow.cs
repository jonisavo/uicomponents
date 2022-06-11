using UnityEditor;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor.Interfaces
{
    public class InterfacesTestEditorWindow : EditorWindow
    {
        private void CreateGUI()
        {
            rootVisualElement.Add(new Label("Hello world"));    
        }

        public void AddTestComponent(UIComponent component)
        {
            rootVisualElement.Add(component);
        }
    }
}