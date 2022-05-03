using UnityEditor;
using UnityEngine;

namespace UIComponentsExamples.Editor
{
    public class ResourcesExampleComponentWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Loading Assets/Resources")]
        private static void ShowWindow()
        {
            var window = GetWindow<ResourcesExampleComponentWindow>();
            window.titleContent = new GUIContent("Loading from Resources");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.style.minWidth = 150;
            rootVisualElement.style.minHeight = 40;
            rootVisualElement.Add(new ResourcesExampleComponent());
        }
    }
}