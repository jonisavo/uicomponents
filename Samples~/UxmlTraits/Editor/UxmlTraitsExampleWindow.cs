using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Samples.UxmlTraits.Editor
{
    public class UxmlTraitsExampleWindow : UnityEditor.EditorWindow
    {
        [UnityEditor.MenuItem("UIComponents Examples/UxmlTraits and UxmlName")]
        private static void ShowWindow()
        {
            var window = GetWindow<UxmlTraitsExampleWindow>();
            window.titleContent = new GUIContent("UxmlTraits and UxmlName");
            window.minSize = new Vector2(470, 250);
            window.Show();
        }

        private void CreateGUI()
        {
            var layout = Resources.Load<VisualTreeAsset>("UxmlTraitsExampleWindow");

            layout.CloneTree(rootVisualElement);

            var styles = Resources.Load<StyleSheet>("UxmlTraitsExampleWindow.style");
            
            rootVisualElement.styleSheets.Add(styles);
        }
    }
}