using UnityEditor;
using UnityEngine;

namespace UIComponentsExamples.Editor
{
    public class AssetDatabaseExampleComponentWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Loading Assets/AssetDatabase")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetDatabaseExampleComponentWindow>();
            window.titleContent = new GUIContent("Loading from AssetDatabase");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.style.minWidth = 150;
            rootVisualElement.style.minHeight = 40;
            rootVisualElement.Add(new AssetDatabaseExampleComponent());
        }
    }
}