using UnityEditor;
using UnityEngine;

namespace UIComponents.Samples.EventInterfaces.Editor
{
    public class EventInterfacesWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Event Interfaces")]
        private static void ShowWindow()
        {
            var window = GetWindow<EventInterfacesWindow>();
            window.titleContent = new GUIContent("Event Interfaces");
            window.minSize = new Vector2(360, 100);
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(new EventInterfacesView());
        }
    }
}

