using UnityEditor;
using UnityEngine;

namespace UIComponents.Samples.Counter.Editor
{
    public class CounterComponentWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Counter")]
        private static void ShowWindow()
        {
            var window = GetWindow<CounterComponentWindow>();
            window.titleContent = new GUIContent("Counter");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.style.minWidth = 120;
            rootVisualElement.style.minHeight = 40;
            rootVisualElement.Add(new CounterComponent());
        }
    }
}