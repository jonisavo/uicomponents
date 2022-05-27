using UnityEditor;
using UnityEngine;

namespace UIComponents.Samples.Query.Editor
{
    public class QueryExampleComponentWindow : EditorWindow
    {
        [MenuItem("UIComponents Examples/Experimental/QueryAttribute")]
        private static void ShowWindow()
        {
            var window = GetWindow<QueryExampleComponentWindow>();
            window.titleContent = new GUIContent("QueryAttribute");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.style.minWidth = 190;
            rootVisualElement.style.minHeight = 40;
            rootVisualElement.Add(new QueryExampleComponent());
        }
    }
}