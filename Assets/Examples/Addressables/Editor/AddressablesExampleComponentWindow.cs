using UnityEditor;
using UnityEngine;

namespace UIComponentsExamples.Addressables.Editor
{
    public class AddressablesExampleComponentWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Loading Assets/Addressables")]
        private static void ShowWindow()
        {
            var window = GetWindow<AddressablesExampleComponentWindow>();
            window.titleContent = new GUIContent("Loading from Addressables");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.style.minWidth = 150;
            rootVisualElement.style.minHeight = 40;
            rootVisualElement.Add(new AddressablesExampleComponent());
        }
    }
}